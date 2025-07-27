using DineProX.Dtos.OrderManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.OrderManagement
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto input);
        Task<OrderDto> GetAsync(Guid id);
        Task<List<OrderDto>> GetListAsync();
        Task CancelAsync(Guid id);
        Task MarkAsPaidAsync(Guid id);
    }
} 