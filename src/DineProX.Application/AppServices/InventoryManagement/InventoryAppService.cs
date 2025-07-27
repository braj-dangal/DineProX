using DineProX.Dtos.InventoryManagement;
using DineProX.Entities.InventoryManagement;
using DineProX.Interfaces.InventoryManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace DineProX.AppServices.InventoryManagement
{
    public class InventoryAppService : ApplicationService, IInventoryAppService
    {
        private readonly IRepository<Inventory, Guid> _inventoryRepository;

        public InventoryAppService(IRepository<Inventory, Guid> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Inventory requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Inventory requested for ID: {id}");

            var inventory = await _inventoryRepository.GetAsync(id);
            return ObjectMapper.Map<Inventory, InventoryDto>(inventory);
        }

        public async Task<List<InventoryDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Inventory List requested by User: {CurrentUser.Id}");

            var inventories = await _inventoryRepository.GetListAsync();
            return ObjectMapper.Map<List<Inventory>, List<InventoryDto>>(inventories);
        }

        public async Task<InventoryDto> UpdateQuantityAsync(UpdateInventoryDto input)
        {
            Logger.LogInformation($"Update Inventory Quantity requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Update Inventory Quantity requested for ID: {input.InventoryId} with change: {input.QuantityChange}");

            // Get the inventory record
            var inventory = await _inventoryRepository.GetAsync(input.InventoryId);

            // Calculate new quantity
            var newQuantity = inventory.QuantityAvailable + input.QuantityChange;

            // Optional: Prevent negative quantities
            if (newQuantity < 0)
            {
                throw new UserFriendlyException($"Cannot reduce inventory below 0. Current quantity: {inventory.QuantityAvailable}, Requested change: {input.QuantityChange}");
            }

            // Update the quantity
            inventory.QuantityAvailable = newQuantity;

            // Save the updated inventory
            var updatedInventory = await _inventoryRepository.UpdateAsync(inventory);

            Logger.LogInformation($"Inventory {input.InventoryId} quantity updated. Old quantity: {inventory.QuantityAvailable - input.QuantityChange}, New quantity: {updatedInventory.QuantityAvailable}");

            return ObjectMapper.Map<Inventory, InventoryDto>(updatedInventory);
        }
    }
} 