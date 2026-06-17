using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AutoMapper;
using DineProX.Entities.MasterData;
using DineProX.Dtos.MasterData;
using DineProX.Interfaces.MasterData.ItemCategory;

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class ItemCategoryAppService : ApplicationService, IItemCategoryAppService
    {
        private readonly IRepository<ItemCategory, Guid> _repository;

        public ItemCategoryAppService(IRepository<ItemCategory, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ItemCategoryDto> CreateAsync(CreateItemCategoryDto input)
        {
            var entity = new ItemCategory(Guid.NewGuid(), input.Name, input.Description, input.DisplayOrder);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(created);
        }

        public async Task<ItemCategoryDto> UpdateAsync(Guid id, UpdateItemCategoryDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.DisplayOrder = input.DisplayOrder;
            entity.IsActive = input.IsActive;
            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ItemCategoryDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(entity);
        }

        public async Task<PagedResultDto<ItemCategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<ItemCategoryDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<ItemCategory, ItemCategoryDto>(x)).ToList()
            );
        }

        public async Task<List<ItemCategoryDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<ItemCategory, ItemCategoryDto>(x)).ToList();
        }
    }
}
