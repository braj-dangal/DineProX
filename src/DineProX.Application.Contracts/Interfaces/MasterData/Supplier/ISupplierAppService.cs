using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.Supplier
{
    public interface ISupplierAppService : IApplicationService
    {
        Task<SupplierDto> CreateAsync(CreateSupplierDto input);
        Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto input);
        Task DeleteAsync(Guid id);
        Task<SupplierDto> GetAsync(Guid id);
        Task<PagedResultDto<SupplierDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<SupplierDto>> GetAllActiveAsync();
        Task<List<SupplierDto>> SearchByNameAsync(string name);
    }
}
