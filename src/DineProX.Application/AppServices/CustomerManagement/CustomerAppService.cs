using DineProX.Dtos.CustomerManagement;
using DineProX.Entities.CustomerManagement;
using DineProX.Interfaces.CustomerManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace DineProX.AppServices.CustomerManagement
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerAppService(IRepository<Customer, Guid> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Customer requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Customer requested for ID: {id}");

            var customer = await _customerRepository.GetAsync(id);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Customer List requested by User: {CurrentUser.Id}");

            var customers = await _customerRepository.GetListAsync();
            return ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            Logger.LogInformation($"Create Customer requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Create Customer requested for: {input}");

            // Check if customer with same phone number already exists
            var existingCustomer = await _customerRepository.FirstOrDefaultAsync(c => c.PhoneNumber == input.PhoneNumber);
            if (existingCustomer != null)
            {
                throw new UserFriendlyException("A customer with this phone number already exists.");
            }

            var customer = new Customer(
                GuidGenerator.Create(),
                input.Name.Trim(),
                input.PhoneNumber.Trim(),
                input.Address.Trim(),
                input.UserId
            );

            var createdCustomer = await _customerRepository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(createdCustomer);
        }

        public async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
        {
            Logger.LogInformation($"Update Customer requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Update Customer requested for ID: {id} with data: {input}");

            var customer = await _customerRepository.GetAsync(id);

            // Check if phone number is being changed and if it conflicts with another customer
            if (customer.PhoneNumber != input.PhoneNumber)
            {
                var existingCustomer = await _customerRepository.FirstOrDefaultAsync(c => c.PhoneNumber == input.PhoneNumber && c.Id != id);
                if (existingCustomer != null)
                {
                    throw new UserFriendlyException("A customer with this phone number already exists.");
                }
            }

            customer.Name = input.Name.Trim();
            customer.PhoneNumber = input.PhoneNumber.Trim();
            customer.Address = input.Address.Trim();
            customer.UserId = input.UserId;

            var updatedCustomer = await _customerRepository.UpdateAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(updatedCustomer);
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"Delete Customer requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Delete Customer requested for ID: {id}");

            await _customerRepository.DeleteAsync(id);
        }
    }
} 