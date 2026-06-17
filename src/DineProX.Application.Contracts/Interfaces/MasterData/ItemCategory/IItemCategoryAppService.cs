using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData.ItemCategory
{
    public interface IItemCategoryAppService : IApplicationService
    {
        Task<ItemCategoryDto> CreateAsync(CreateItemCategoryDto input);
        Task<ItemCategoryDto> UpdateAsync(Guid id, UpdateItemCategoryDto input);
        Task DeleteAsync(Guid id);
        Task<ItemCategoryDto> GetAsync(Guid id);
        Task<PagedResultDto<ItemCategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<ItemCategoryDto>> GetAllActiveAsync();
    }
}
