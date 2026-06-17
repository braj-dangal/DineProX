using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.PaymentMethod
{
    public interface IPaymentMethodAppService : IApplicationService
    {
        Task<PaymentMethodDto> CreateAsync(CreatePaymentMethodDto input);
        Task<PaymentMethodDto> UpdateAsync(Guid id, UpdatePaymentMethodDto input);
        Task DeleteAsync(Guid id);
        Task<PaymentMethodDto> GetAsync(Guid id);
        Task<PagedResultDto<PaymentMethodDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<PaymentMethodDto>> GetAllActiveAsync();
    }
}
