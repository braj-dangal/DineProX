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

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class TableZoneAppService : ApplicationService
    {
        private readonly IRepository<TableZone, Guid> _repository;

        public TableZoneAppService(IRepository<TableZone, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<TableZoneDto> CreateAsync(CreateTableZoneDto input)
        {
            var entity = new TableZone(Guid.NewGuid(), input.Name, input.Description);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<TableZone, TableZoneDto>(created);
        }

        public async Task<TableZoneDto> UpdateAsync(Guid id, UpdateTableZoneDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<TableZone, TableZoneDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<TableZoneDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<TableZone, TableZoneDto>(entity);
        }

        public async Task<PagedResultDto<TableZoneDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TableZoneDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<TableZone, TableZoneDto>(x)).ToList()
            );
        }

        public async Task<List<TableZoneDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<TableZone, TableZoneDto>(x)).ToList();
        }
    }
}
