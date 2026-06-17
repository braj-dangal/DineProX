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
using DineProX.Interfaces.MasterData.Table;

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class TableAppService : ApplicationService, ITableAppService
    {
        private readonly IRepository<Table, Guid> _repository;

        public TableAppService(IRepository<Table, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<TableDto> CreateAsync(CreateTableDto input)
        {
            var entity = new Table(Guid.NewGuid(), input.TableNumber, input.Capacity, input.ZoneId);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<Table, TableDto>(created);
        }

        public async Task<TableDto> UpdateAsync(Guid id, UpdateTableDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.TableNumber = input.TableNumber;
            entity.Capacity = input.Capacity;
            entity.ZoneId = input.ZoneId;
            entity.Status = (TableStatus)input.Status;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<Table, TableDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<TableDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<Table, TableDto>(entity);
        }

        public async Task<PagedResultDto<TableDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TableDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList()
            );
        }

        public async Task<List<TableDto>> GetByZoneAsync(Guid zoneId)
        {
            var items = await _repository.GetListAsync(x => x.ZoneId == zoneId && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList();
        }

        public async Task<List<TableDto>> GetByStatusAsync(int status)
        {
            var tableStatus = (TableStatus)status;
            var items = await _repository.GetListAsync(x => x.Status == tableStatus && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList();
        }

        public async Task MarkAsOccupiedAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            entity.MarkAsOccupied();
            await _repository.UpdateAsync(entity);
        }

        public async Task MarkAsFreeAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            entity.MarkAsFree();
            await _repository.UpdateAsync(entity);
        }

        public async Task MarkAsReservedAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            entity.MarkAsReserved();
            await _repository.UpdateAsync(entity);
        }
    }
}
