using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.TaxRate
{
    public interface ITaxRateAppService : IApplicationService
    {
        Task<TaxRateDto> CreateAsync(CreateTaxRateDto input);
        Task<TaxRateDto> UpdateAsync(Guid id, UpdateTaxRateDto input);
        Task DeleteAsync(Guid id);
        Task<TaxRateDto> GetAsync(Guid id);
        Task<PagedResultDto<TaxRateDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TaxRateDto>> GetAllActiveAsync();
    }
}
