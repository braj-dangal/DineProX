using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.Shift
{
    public interface IShiftAppService : IApplicationService
    {
        Task<ShiftDto> CreateAsync(CreateShiftDto input);
        Task<ShiftDto> UpdateAsync(Guid id, UpdateShiftDto input);
        Task DeleteAsync(Guid id);
        Task<ShiftDto> GetAsync(Guid id);
        Task<PagedResultDto<ShiftDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<ShiftDto>> GetAllActiveAsync();
    }
}
