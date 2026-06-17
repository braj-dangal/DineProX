using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.Table
{
    public interface ITableAppService : IApplicationService
    {
        Task<TableDto> CreateAsync(CreateTableDto input);
        Task<TableDto> UpdateAsync(Guid id, UpdateTableDto input);
        Task DeleteAsync(Guid id);
        Task<TableDto> GetAsync(Guid id);
        Task<PagedResultDto<TableDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TableDto>> GetByZoneAsync(Guid zoneId);
        Task<List<TableDto>> GetByStatusAsync(int status);
        Task MarkAsOccupiedAsync(Guid id);
        Task MarkAsFreeAsync(Guid id);
        Task MarkAsReservedAsync(Guid id);
    }
}
