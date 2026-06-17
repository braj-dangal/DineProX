using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.TableZone
{
    public interface ITableZoneAppService : IApplicationService
    {
        Task<TableZoneDto> CreateAsync(CreateTableZoneDto input);
        Task<TableZoneDto> UpdateAsync(Guid id, UpdateTableZoneDto input);
        Task DeleteAsync(Guid id);
        Task<TableZoneDto> GetAsync(Guid id);
        Task<PagedResultDto<TableZoneDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TableZoneDto>> GetAllActiveAsync();
    }
}
