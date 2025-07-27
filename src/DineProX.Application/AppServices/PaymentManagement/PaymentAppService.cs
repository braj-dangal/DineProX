using DineProX.Dtos.PaymentManagement;
using DineProX.Entities.PaymentManagement;
using DineProX.Interfaces.PaymentManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace DineProX.AppServices.PaymentManagement
{
    public class PaymentAppService : ApplicationService, IPaymentAppService
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IRepository<Due, Guid> _dueRepository;
        private readonly DomainService _domainService;

        public PaymentAppService(
            IRepository<Payment, Guid> paymentRepository,
            IRepository<Due, Guid> dueRepository,
            DomainService domainService)
        {
            _paymentRepository = paymentRepository;
            _dueRepository = dueRepository;
            _domainService = domainService;
        }

        public async Task<PaymentDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Payment requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Payment requested for ID: {id}");

            var payment = await _paymentRepository.GetAsync(id);
            return ObjectMapper.Map<Payment, PaymentDto>(payment);
        }

        public async Task<List<PaymentDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Payment List requested by User: {CurrentUser.Id}");

            var payments = await _paymentRepository.GetListAsync();
            return ObjectMapper.Map<List<Payment>, List<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> CreateAsync(CreateUpdatePaymentDto input)
        {
            Logger.LogInformation($"Create Payment requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Create Payment requested for: {input}");

            // Validate business rules
            if (input.AmountPaid < 0)
            {
                throw new UserFriendlyException("Amount paid cannot be negative.");
            }

            if (input.Discount < 0)
            {
                throw new UserFriendlyException("Discount cannot be negative.");
            }

            if (input.TotalBill < 0)
            {
                throw new UserFriendlyException("Total bill cannot be negative.");
            }

            if (input.Discount > input.TotalBill)
            {
                throw new UserFriendlyException("Discount cannot be greater than total bill.");
            }

            // Calculate total amount after discount
            var totalAmount = input.TotalBill - input.Discount;

            // Create the payment
            var payment = new Payment(
                GuidGenerator.Create(),
                input.OrderId,
                input.CustomerId,
                input.AmountPaid,
                input.Discount,
                input.TotalBill,
                input.Date
            );

            // Insert payment
            var createdPayment = await _paymentRepository.InsertAsync(payment);

            // Check if there's a due amount and create Due entry if needed
            if (input.AmountPaid < totalAmount)
            {
                var remainingDue = totalAmount - input.AmountPaid;
                
                var due = new Due(
                    GuidGenerator.Create(),
                    createdPayment.Id,
                    input.CustomerId,
                    totalAmount,
                    input.AmountPaid,
                    remainingDue,
                    input.Date.AddDays(30), // Default due date 30 days from payment date
                    false
                );

                await _dueRepository.InsertAsync(due);
            }

            return ObjectMapper.Map<Payment, PaymentDto>(createdPayment);
        }
    }
} 