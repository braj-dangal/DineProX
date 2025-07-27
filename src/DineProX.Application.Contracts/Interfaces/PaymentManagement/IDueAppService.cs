using DineProX.Dtos.PaymentManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.PaymentManagement
{
    public interface IDueAppService : IApplicationService
    {
        Task<List<DueDto>> GetUnsettledListAsync();
        Task<DueDto> GetAsync(Guid id);
        Task<DueDto> SettleAsync(SettleDueDto input);
    }
} 