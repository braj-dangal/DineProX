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
    public class TaxRateAppService : ApplicationService
    {
        private readonly IRepository<TaxRate, Guid> _repository;

        public TaxRateAppService(IRepository<TaxRate, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<TaxRateDto> CreateAsync(CreateTaxRateDto input)
        {
            var entity = new TaxRate(Guid.NewGuid(), input.Name, input.Rate, input.Description);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(created);
        }

        public async Task<TaxRateDto> UpdateAsync(Guid id, UpdateTaxRateDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.Rate = input.Rate;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<TaxRateDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(entity);
        }

        public async Task<PagedResultDto<TaxRateDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TaxRateDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<TaxRate, TaxRateDto>(x)).ToList()
            );
        }

        public async Task<List<TaxRateDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<TaxRate, TaxRateDto>(x)).ToList();
        }
    }
}
