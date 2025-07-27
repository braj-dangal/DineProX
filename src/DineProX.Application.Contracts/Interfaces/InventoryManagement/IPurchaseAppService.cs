using DineProX.Dtos.InventoryManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.InventoryManagement
{
    public interface IPurchaseAppService : IApplicationService
    {
        Task<PurchaseDto> CreateAsync(CreatePurchaseDto input);
        Task<List<PurchaseDto>> GetListAsync();
        Task<PurchaseDto> GetAsync(Guid id);
    }
} 