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
    public class ShiftAppService : ApplicationService
    {
        private readonly IRepository<Shift, Guid> _repository;

        public ShiftAppService(IRepository<Shift, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ShiftDto> CreateAsync(CreateShiftDto input)
        {
            var startTime = TimeSpan.Parse(input.StartTime);
            var endTime = TimeSpan.Parse(input.EndTime);
            var entity = new Shift(Guid.NewGuid(), input.Name, startTime, endTime, input.Description);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<Shift, ShiftDto>(created);
        }

        public async Task<ShiftDto> UpdateAsync(Guid id, UpdateShiftDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.StartTime = TimeSpan.Parse(input.StartTime);
            entity.EndTime = TimeSpan.Parse(input.EndTime);
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<Shift, ShiftDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ShiftDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<Shift, ShiftDto>(entity);
        }

        public async Task<PagedResultDto<ShiftDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<ShiftDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<Shift, ShiftDto>(x)).ToList()
            );
        }

        public async Task<List<ShiftDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<Shift, ShiftDto>(x)).ToList();
        }
    }
}
