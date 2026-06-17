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
using DineProX.Interfaces.MasterData.PaymentMethod;

namespace DineProX.AppServices.MasterData
{
    [Authorize]
    public class PaymentMethodAppService : ApplicationService, IPaymentMethodAppService
    {
        private readonly IRepository<PaymentMethod, Guid> _repository;

        public PaymentMethodAppService(IRepository<PaymentMethod, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PaymentMethodDto> CreateAsync(CreatePaymentMethodDto input)
        {
            var entity = new PaymentMethod(Guid.NewGuid(), input.Name, (PaymentType)input.Type, input.Description);
            var created = await _repository.InsertAsync(entity);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(created);
        }

        public async Task<PaymentMethodDto> UpdateAsync(Guid id, UpdatePaymentMethodDto input)
        {
            var entity = await _repository.GetAsync(id);
            entity.Name = input.Name;
            entity.Type = (PaymentType)input.Type;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;

            var updated = await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<PaymentMethodDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(entity);
        }

        public async Task<PagedResultDto<PaymentMethodDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<PaymentMethodDto>(
                totalCount,
                items.Select(x => ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(x)).ToList()
            );
        }

        public async Task<List<PaymentMethodDto>> GetAllActiveAsync()
        {
            var items = await _repository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(x)).ToList();
        }
    }
}
