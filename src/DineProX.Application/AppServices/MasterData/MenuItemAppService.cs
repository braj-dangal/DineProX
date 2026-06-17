using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DineProX.Entities.MasterData;
using DineProX.Dtos.MasterData;
using DineProX.Interfaces.MasterData.MenuItem;

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class MenuItemAppService : ApplicationService, IMenuItemAppService
    {
        private readonly IRepository<MenuItem, Guid> _repository;

        public MenuItemAppService(IRepository<MenuItem, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<MenuItemDto> CreateAsync(CreateMenuItemDto input)
        {
            var entity = new MenuItem(
                Guid.NewGuid(),
                input.Name,
                input.CategoryId,
                input.Price,
                input.TaxPercentage,
                input.StockUnit,
                input.ReorderLevel
            );
            entity.StockQuantity = input.StockQuantity;
            entity.Description = input.Description;
            entity.ImageUrl = input.ImageUrl;
            entity.Allergens = input.Allergens;

            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(created);
        }

        public async Task<MenuItemDto> UpdateAsync(Guid id, UpdateMenuItemDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.CategoryId = input.CategoryId;
            entity.Price = input.Price;
            entity.TaxPercentage = input.TaxPercentage;
            entity.StockUnit = input.StockUnit;
            entity.StockQuantity = input.StockQuantity;
            entity.ReorderLevel = input.ReorderLevel;
            entity.Description = input.Description;
            entity.ImageUrl = input.ImageUrl;
            entity.Allergens = input.Allergens;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<MenuItemDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(entity);
        }

        public async Task<PagedResultDto<MenuItemDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<MenuItemDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList()
            );
        }

        public async Task<List<MenuItemDto>> GetByCategoryAsync(Guid categoryId)
        {
            var items = await _repository.GetListAsync(x => x.CategoryId == categoryId && x.IsActive);
            return items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList();
        }

        public async Task<List<MenuItemDto>> GetLowStockAsync()
        {
            var items = await _repository.GetListAsync(x => x.StockQuantity <= x.ReorderLevel && x.IsActive);
            return items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList();
        }

        public async Task AdjustStockAsync(Guid id, int quantity)
        {
            var entity = await _repository.GetAsync(id);
            entity.StockQuantity += quantity;
            await _repository.UpdateAsync(entity);
        }
    }
}
