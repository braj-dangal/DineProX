using DineProX.Dtos.PaymentManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.PaymentManagement
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentDto> CreateAsync(CreateUpdatePaymentDto input);
        Task<List<PaymentDto>> GetListAsync();
        Task<PaymentDto> GetAsync(Guid id);
    }
} 