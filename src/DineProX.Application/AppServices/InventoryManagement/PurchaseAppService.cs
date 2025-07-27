using DineProX.Dtos.InventoryManagement;
using DineProX.Entities.InventoryManagement;
using DineProX.Interfaces.InventoryManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace DineProX.AppServices.InventoryManagement
{
    public class PurchaseAppService : ApplicationService, IPurchaseAppService
    {
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;

        public PurchaseAppService(
            IRepository<Purchase, Guid> purchaseRepository,
            IRepository<Inventory, Guid> inventoryRepository)
        {
            _purchaseRepository = purchaseRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<PurchaseDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Purchase requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Purchase requested for ID: {id}");

            var purchase = await _purchaseRepository.GetAsync(id);
            return ObjectMapper.Map<Purchase, PurchaseDto>(purchase);
        }

        public async Task<List<PurchaseDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Purchase List requested by User: {CurrentUser.Id}");

            var purchases = await _purchaseRepository.GetListAsync();
            return ObjectMapper.Map<List<Purchase>, List<PurchaseDto>>(purchases);
        }

        public async Task<PurchaseDto> CreateAsync(CreatePurchaseDto input)
        {
            Logger.LogInformation($"Create Purchase requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Create Purchase requested for: {input}");

            // Validate business rules
            if (input.Quantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be greater than 0.");
            }

            if (input.PurchasePrice <= 0)
            {
                throw new UserFriendlyException("Purchase price must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(input.SupplierName))
            {
                throw new UserFriendlyException("Supplier name is required.");
            }

            // Create the purchase
            var purchase = new Purchase(
                GuidGenerator.Create(),
                input.DishId,
                input.Quantity,
                input.PurchasePrice,
                input.SupplierName.Trim(),
                input.PurchaseDate
            );

            // Insert the purchase
            var createdPurchase = await _purchaseRepository.InsertAsync(purchase);

            // Update inventory
            await UpdateInventoryAsync(input.DishId, input.Quantity);

            Logger.LogInformation($"Purchase created successfully. Purchase ID: {createdPurchase.Id}, Dish ID: {input.DishId}, Quantity: {input.Quantity}");

            return ObjectMapper.Map<Purchase, PurchaseDto>(createdPurchase);
        }

        private async Task UpdateInventoryAsync(Guid dishId, int quantity)
        {
            // Check if inventory record exists for the dish
            var existingInventory = await _inventoryRepository.FirstOrDefaultAsync(i => i.DishId == dishId);

            if (existingInventory != null)
            {
                // Update existing inventory
                existingInventory.QuantityAvailable += quantity;
                await _inventoryRepository.UpdateAsync(existingInventory);
                
                Logger.LogInformation($"Updated existing inventory for Dish ID: {dishId}. New quantity: {existingInventory.QuantityAvailable}");
            }
            else
            {
                // Create new inventory record
                var newInventory = new Inventory(
                    GuidGenerator.Create(),
                    dishId,
                    quantity
                );

                await _inventoryRepository.InsertAsync(newInventory);
                
                Logger.LogInformation($"Created new inventory record for Dish ID: {dishId}. Quantity: {quantity}");
            }
        }
    }
} 