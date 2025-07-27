using DineProX.Dtos.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.InventoryManagement
{
    public interface IInventoryAppService : IApplicationService
    {
        Task<List<InventoryDto>> GetListAsync();
        Task<InventoryDto> GetAsync(Guid id);
        Task<InventoryDto> UpdateQuantityAsync(UpdateInventoryDto input);
    }
} 