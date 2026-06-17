using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.MenuItem
{
    public interface IMenuItemAppService : IApplicationService
    {
        Task<MenuItemDto> CreateAsync(CreateMenuItemDto input);
        Task<MenuItemDto> UpdateAsync(Guid id, UpdateMenuItemDto input);
        Task DeleteAsync(Guid id);
        Task<MenuItemDto> GetAsync(Guid id);
        Task<PagedResultDto<MenuItemDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<MenuItemDto>> GetByCategoryAsync(Guid categoryId);
        Task<List<MenuItemDto>> GetLowStockAsync();
        Task AdjustStockAsync(Guid id, int quantity);
    }
}
