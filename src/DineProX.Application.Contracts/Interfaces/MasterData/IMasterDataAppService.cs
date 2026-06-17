using DineProX.Dtos.MasterData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.MasterData
{
    public interface IMasterDataAppService : IApplicationService
    {
        Task<ItemCategoryDto> CreateItemCategoryAsync(CreateItemCategoryDto input);
        Task<ItemCategoryDto> UpdateItemCategoryAsync(Guid id, UpdateItemCategoryDto input);
        Task DeleteItemCategoryAsync(Guid id);
        Task<ItemCategoryDto> GetItemCategoryAsync(Guid id);
        Task<PagedResultDto<ItemCategoryDto>> GetItemCategoryListAsync(PagedAndSortedResultRequestDto input);
        Task<List<ItemCategoryDto>> GetAllActiveItemCategoriesAsync();

        Task<MenuItemDto> CreateMenuItemAsync(CreateMenuItemDto input);
        Task<MenuItemDto> UpdateMenuItemAsync(Guid id, UpdateMenuItemDto input);
        Task DeleteMenuItemAsync(Guid id);
        Task<MenuItemDto> GetMenuItemAsync(Guid id);
        Task<PagedResultDto<MenuItemDto>> GetMenuItemListAsync(PagedAndSortedResultRequestDto input);
        Task<List<MenuItemDto>> GetMenuItemsByCategoryAsync(Guid categoryId);
        Task<List<MenuItemDto>> GetLowStockMenuItemsAsync();
        Task AdjustMenuItemStockAsync(Guid id, int quantity);

        Task<TableDto> CreateTableAsync(CreateTableDto input);
        Task<TableDto> UpdateTableAsync(Guid id, UpdateTableDto input);
        Task DeleteTableAsync(Guid id);
        Task<TableDto> GetTableAsync(Guid id);
        Task<PagedResultDto<TableDto>> GetTableListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TableDto>> GetTablesByZoneAsync(Guid zoneId);
        Task<List<TableDto>> GetTablesByStatusAsync(int status);
        Task MarkTableAsOccupiedAsync(Guid id);
        Task MarkTableAsFreeAsync(Guid id);
        Task MarkTableAsReservedAsync(Guid id);

        Task<TableZoneDto> CreateTableZoneAsync(CreateTableZoneDto input);
        Task<TableZoneDto> UpdateTableZoneAsync(Guid id, UpdateTableZoneDto input);
        Task DeleteTableZoneAsync(Guid id);
        Task<TableZoneDto> GetTableZoneAsync(Guid id);
        Task<PagedResultDto<TableZoneDto>> GetTableZoneListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TableZoneDto>> GetAllActiveTableZonesAsync();

        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto input);
        Task<SupplierDto> UpdateSupplierAsync(Guid id, UpdateSupplierDto input);
        Task DeleteSupplierAsync(Guid id);
        Task<SupplierDto> GetSupplierAsync(Guid id);
        Task<PagedResultDto<SupplierDto>> GetSupplierListAsync(PagedAndSortedResultRequestDto input);
        Task<List<SupplierDto>> GetAllActiveSuppliersAsync();
        Task<List<SupplierDto>> SearchSuppliersByNameAsync(string name);

        Task<TaxRateDto> CreateTaxRateAsync(CreateTaxRateDto input);
        Task<TaxRateDto> UpdateTaxRateAsync(Guid id, UpdateTaxRateDto input);
        Task DeleteTaxRateAsync(Guid id);
        Task<TaxRateDto> GetTaxRateAsync(Guid id);
        Task<PagedResultDto<TaxRateDto>> GetTaxRateListAsync(PagedAndSortedResultRequestDto input);
        Task<List<TaxRateDto>> GetAllActiveTaxRatesAsync();

        Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodDto input);
        Task<PaymentMethodDto> UpdatePaymentMethodAsync(Guid id, UpdatePaymentMethodDto input);
        Task DeletePaymentMethodAsync(Guid id);
        Task<PaymentMethodDto> GetPaymentMethodAsync(Guid id);
        Task<PagedResultDto<PaymentMethodDto>> GetPaymentMethodListAsync(PagedAndSortedResultRequestDto input);
        Task<List<PaymentMethodDto>> GetAllActivePaymentMethodsAsync();

        Task<ShiftDto> CreateShiftAsync(CreateShiftDto input);
        Task<ShiftDto> UpdateShiftAsync(Guid id, UpdateShiftDto input);
        Task DeleteShiftAsync(Guid id);
        Task<ShiftDto> GetShiftAsync(Guid id);
        Task<PagedResultDto<ShiftDto>> GetShiftListAsync(PagedAndSortedResultRequestDto input);
        Task<List<ShiftDto>> GetAllActiveShiftsAsync();
    }
}
