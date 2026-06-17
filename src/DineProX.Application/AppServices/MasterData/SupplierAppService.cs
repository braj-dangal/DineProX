using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DineProX.Entities.MasterData;
using DineProX.Dtos.MasterData;
using DineProX.Interfaces.MasterData.Supplier;

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class SupplierAppService : ApplicationService, ISupplierAppService
    {
        private readonly IRepository<Supplier, Guid> _repository;

        public SupplierAppService(IRepository<Supplier, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierDto input)
        {
            var entity = new Supplier(Guid.NewGuid(), input.Name, input.ContactPerson, input.Email);
            entity.Phone = input.Phone;
            entity.Address = input.Address;
            entity.City = input.City;
            entity.PostalCode = input.PostalCode;
            entity.Country = input.Country;
            entity.PaymentTerms = input.PaymentTerms;
            entity.CreditLimit = input.CreditLimit;

            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<Supplier, SupplierDto>(created);
        }

        public async Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.ContactPerson = input.ContactPerson;
            entity.Email = input.Email;
            entity.Phone = input.Phone;
            entity.Address = input.Address;
            entity.City = input.City;
            entity.PostalCode = input.PostalCode;
            entity.Country = input.Country;
            entity.PaymentTerms = input.PaymentTerms;
            entity.CreditLimit = input.CreditLimit;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<Supplier, SupplierDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<SupplierDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<Supplier, SupplierDto>(entity);
        }

        public async Task<PagedResultDto<SupplierDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<SupplierDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList()
            );
        }

        public async Task<List<SupplierDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList();
        }

        public async Task<List<SupplierDto>> SearchByNameAsync(string name)
        {
            var items = await _repository.GetListAsync(x => x.Name.Contains(name) && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList();
        }
    }
}
