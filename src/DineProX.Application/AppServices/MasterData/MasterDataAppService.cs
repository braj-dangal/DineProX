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
    public class MasterDataAppService : ApplicationService
    {
        private readonly IRepository<ItemCategory, Guid> _itemCategoryRepository;
        private readonly IRepository<MenuItem, Guid> _menuItemRepository;
        private readonly IRepository<Table, Guid> _tableRepository;
        private readonly IRepository<TableZone, Guid> _tableZoneRepository;
        private readonly IRepository<Supplier, Guid> _supplierRepository;
        private readonly IRepository<TaxRate, Guid> _taxRateRepository;
        private readonly IRepository<PaymentMethod, Guid> _paymentMethodRepository;
        private readonly IRepository<Shift, Guid> _shiftRepository;

        public MasterDataAppService(
            IRepository<ItemCategory, Guid> itemCategoryRepository,
            IRepository<MenuItem, Guid> menuItemRepository,
            IRepository<Table, Guid> tableRepository,
            IRepository<TableZone, Guid> tableZoneRepository,
            IRepository<Supplier, Guid> supplierRepository,
            IRepository<TaxRate, Guid> taxRateRepository,
            IRepository<PaymentMethod, Guid> paymentMethodRepository,
            IRepository<Shift, Guid> shiftRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _menuItemRepository = menuItemRepository;
            _tableRepository = tableRepository;
            _tableZoneRepository = tableZoneRepository;
            _supplierRepository = supplierRepository;
            _taxRateRepository = taxRateRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _shiftRepository = shiftRepository;
        }

        // ItemCategory
        public async Task<ItemCategoryDto> CreateItemCategoryAsync(CreateItemCategoryDto input)
        {
            var entity = new ItemCategory(Guid.NewGuid(), input.Name, input.Description, input.DisplayOrder);
            var created = await _itemCategoryRepository.InsertAsync(entity);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(created);
        }

        public async Task<ItemCategoryDto> UpdateItemCategoryAsync(Guid id, UpdateItemCategoryDto input)
        {
            var entity = await _itemCategoryRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.DisplayOrder = input.DisplayOrder;
            entity.IsActive = input.IsActive;
            var updated = await _itemCategoryRepository.UpdateAsync(entity);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(updated);
        }

        public async Task DeleteItemCategoryAsync(Guid id)
        {
            await _itemCategoryRepository.DeleteAsync(id);
        }

        public async Task<ItemCategoryDto> GetItemCategoryAsync(Guid id)
        {
            var entity = await _itemCategoryRepository.GetAsync(id);
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(entity);
        }

        public async Task<PagedResultDto<ItemCategoryDto>> GetItemCategoryListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _itemCategoryRepository.CountAsync();
            var items = await _itemCategoryRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<ItemCategoryDto>(totalCount, items.Select(x => ObjectMapper.Map<ItemCategory, ItemCategoryDto>(x)).ToList());
        }

        public async Task<List<ItemCategoryDto>> GetAllActiveItemCategoriesAsync()
        {
            var items = await _itemCategoryRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<ItemCategory, ItemCategoryDto>(x)).ToList();
        }

        // MenuItem
        public async Task<MenuItemDto> CreateMenuItemAsync(CreateMenuItemDto input)
        {
            var entity = new MenuItem(
                Guid.NewGuid(),
                input.Name,
                input.CategoryId,
                input.Price,
                input.TaxPercentage,
                input.StockUnit,
                input.ReorderLevel);
            entity.StockQuantity = input.StockQuantity;
            entity.Description = input.Description;
            entity.ImageUrl = input.ImageUrl;
            entity.Allergens = input.Allergens;
            var created = await _menuItemRepository.InsertAsync(entity);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(created);
        }

        public async Task<MenuItemDto> UpdateMenuItemAsync(Guid id, UpdateMenuItemDto input)
        {
            var entity = await _menuItemRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.CategoryId = input.CategoryId;
            entity.Price = input.Price;
            entity.TaxPercentage = input.TaxPercentage;
            entity.StockUnit = input.StockUnit;
            entity.StockQuantity = input.StockQuantity;
            entity.ReorderLevel = input.ReorderLevel;
            entity.Description = input.Description;
            entity.ImageUrl = input.ImageUrl;
            entity.Allergens = input.Allergens;
            entity.IsActive = input.IsActive;
            var updated = await _menuItemRepository.UpdateAsync(entity);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(updated);
        }

        public async Task DeleteMenuItemAsync(Guid id)
        {
            await _menuItemRepository.DeleteAsync(id);
        }

        public async Task<MenuItemDto> GetMenuItemAsync(Guid id)
        {
            var entity = await _menuItemRepository.GetAsync(id);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(entity);
        }

        public async Task<PagedResultDto<MenuItemDto>> GetMenuItemListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _menuItemRepository.CountAsync();
            var items = await _menuItemRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<MenuItemDto>(totalCount, items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList());
        }

        public async Task<List<MenuItemDto>> GetMenuItemsByCategoryAsync(Guid categoryId)
        {
            var items = await _menuItemRepository.GetListAsync(x => x.CategoryId == categoryId && x.IsActive);
            return items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList();
        }

        public async Task<List<MenuItemDto>> GetLowStockMenuItemsAsync()
        {
            var items = await _menuItemRepository.GetListAsync(x => x.StockQuantity <= x.ReorderLevel && x.IsActive);
            return items.Select(x => ObjectMapper.Map<MenuItem, MenuItemDto>(x)).ToList();
        }

        public async Task AdjustMenuItemStockAsync(Guid id, int quantity)
        {
            var entity = await _menuItemRepository.GetAsync(id);
            entity.StockQuantity += quantity;
            await _menuItemRepository.UpdateAsync(entity);
        }

        // Table
        public async Task<TableDto> CreateTableAsync(CreateTableDto input)
        {
            var entity = new Table(Guid.NewGuid(), input.TableNumber, input.Capacity, input.ZoneId);
            var created = await _tableRepository.InsertAsync(entity);
            return ObjectMapper.Map<Table, TableDto>(created);
        }

        public async Task<TableDto> UpdateTableAsync(Guid id, UpdateTableDto input)
        {
            var entity = await _tableRepository.GetAsync(id);
            entity.TableNumber = input.TableNumber;
            entity.Capacity = input.Capacity;
            entity.ZoneId = input.ZoneId;
            entity.Status = (TableStatus)input.Status;
            entity.IsActive = input.IsActive;
            var updated = await _tableRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Table, TableDto>(updated);
        }

        public async Task DeleteTableAsync(Guid id)
        {
            await _tableRepository.DeleteAsync(id);
        }

        public async Task<TableDto> GetTableAsync(Guid id)
        {
            var entity = await _tableRepository.GetAsync(id);
            return ObjectMapper.Map<Table, TableDto>(entity);
        }

        public async Task<PagedResultDto<TableDto>> GetTableListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _tableRepository.CountAsync();
            var items = await _tableRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TableDto>(totalCount, items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList());
        }

        public async Task<List<TableDto>> GetTablesByZoneAsync(Guid zoneId)
        {
            var items = await _tableRepository.GetListAsync(x => x.ZoneId == zoneId && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList();
        }

        public async Task<List<TableDto>> GetTablesByStatusAsync(int status)
        {
            var tableStatus = (TableStatus)status;
            var items = await _tableRepository.GetListAsync(x => x.Status == tableStatus && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Table, TableDto>(x)).ToList();
        }

        public async Task MarkTableAsOccupiedAsync(Guid id)
        {
            var entity = await _tableRepository.GetAsync(id);
            entity.MarkAsOccupied();
            await _tableRepository.UpdateAsync(entity);
        }

        public async Task MarkTableAsFreeAsync(Guid id)
        {
            var entity = await _tableRepository.GetAsync(id);
            entity.MarkAsFree();
            await _tableRepository.UpdateAsync(entity);
        }

        public async Task MarkTableAsReservedAsync(Guid id)
        {
            var entity = await _tableRepository.GetAsync(id);
            entity.MarkAsReserved();
            await _tableRepository.UpdateAsync(entity);
        }

        // TableZone
        public async Task<TableZoneDto> CreateTableZoneAsync(CreateTableZoneDto input)
        {
            var entity = new TableZone(Guid.NewGuid(), input.Name, input.Description);
            var created = await _tableZoneRepository.InsertAsync(entity);
            return ObjectMapper.Map<TableZone, TableZoneDto>(created);
        }

        public async Task<TableZoneDto> UpdateTableZoneAsync(Guid id, UpdateTableZoneDto input)
        {
            var entity = await _tableZoneRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;
            var updated = await _tableZoneRepository.UpdateAsync(entity);
            return ObjectMapper.Map<TableZone, TableZoneDto>(updated);
        }

        public async Task DeleteTableZoneAsync(Guid id)
        {
            await _tableZoneRepository.DeleteAsync(id);
        }

        public async Task<TableZoneDto> GetTableZoneAsync(Guid id)
        {
            var entity = await _tableZoneRepository.GetAsync(id);
            return ObjectMapper.Map<TableZone, TableZoneDto>(entity);
        }

        public async Task<PagedResultDto<TableZoneDto>> GetTableZoneListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _tableZoneRepository.CountAsync();
            var items = await _tableZoneRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TableZoneDto>(totalCount, items.Select(x => ObjectMapper.Map<TableZone, TableZoneDto>(x)).ToList());
        }

        public async Task<List<TableZoneDto>> GetAllActiveTableZonesAsync()
        {
            var items = await _tableZoneRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<TableZone, TableZoneDto>(x)).ToList();
        }

        // Supplier
        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto input)
        {
            var entity = new Supplier(Guid.NewGuid(), input.Name, input.ContactPerson, input.Email);
            entity.Phone = input.Phone;
            entity.Address = input.Address;
            entity.City = input.City;
            entity.PostalCode = input.PostalCode;
            entity.Country = input.Country;
            entity.PaymentTerms = input.PaymentTerms;
            entity.CreditLimit = input.CreditLimit;
            var created = await _supplierRepository.InsertAsync(entity);
            return ObjectMapper.Map<Supplier, SupplierDto>(created);
        }

        public async Task<SupplierDto> UpdateSupplierAsync(Guid id, UpdateSupplierDto input)
        {
            var entity = await _supplierRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.ContactPerson = input.ContactPerson;
            entity.Email = input.Email;
            entity.Phone = input.Phone;
            entity.Address = input.Address;
            entity.City = input.City;
            entity.PostalCode = input.PostalCode;
            entity.Country = input.Country;
            entity.PaymentTerms = input.PaymentTerms;
            entity.CreditLimit = input.CreditLimit;
            entity.IsActive = input.IsActive;
            var updated = await _supplierRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Supplier, SupplierDto>(updated);
        }

        public async Task DeleteSupplierAsync(Guid id)
        {
            await _supplierRepository.DeleteAsync(id);
        }

        public async Task<SupplierDto> GetSupplierAsync(Guid id)
        {
            var entity = await _supplierRepository.GetAsync(id);
            return ObjectMapper.Map<Supplier, SupplierDto>(entity);
        }

        public async Task<PagedResultDto<SupplierDto>> GetSupplierListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _supplierRepository.CountAsync();
            var items = await _supplierRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<SupplierDto>(totalCount, items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList());
        }

        public async Task<List<SupplierDto>> GetAllActiveSuppliersAsync()
        {
            var items = await _supplierRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList();
        }

        public async Task<List<SupplierDto>> SearchSuppliersByNameAsync(string name)
        {
            var items = await _supplierRepository.GetListAsync(x => x.Name.Contains(name) && x.IsActive);
            return items.Select(x => ObjectMapper.Map<Supplier, SupplierDto>(x)).ToList();
        }

        // TaxRate
        public async Task<TaxRateDto> CreateTaxRateAsync(CreateTaxRateDto input)
        {
            var entity = new TaxRate(Guid.NewGuid(), input.Name, input.Rate, input.Description);
            var created = await _taxRateRepository.InsertAsync(entity);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(created);
        }

        public async Task<TaxRateDto> UpdateTaxRateAsync(Guid id, UpdateTaxRateDto input)
        {
            var entity = await _taxRateRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.Rate = input.Rate;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;
            var updated = await _taxRateRepository.UpdateAsync(entity);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(updated);
        }

        public async Task DeleteTaxRateAsync(Guid id)
        {
            await _taxRateRepository.DeleteAsync(id);
        }

        public async Task<TaxRateDto> GetTaxRateAsync(Guid id)
        {
            var entity = await _taxRateRepository.GetAsync(id);
            return ObjectMapper.Map<TaxRate, TaxRateDto>(entity);
        }

        public async Task<PagedResultDto<TaxRateDto>> GetTaxRateListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _taxRateRepository.CountAsync();
            var items = await _taxRateRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<TaxRateDto>(totalCount, items.Select(x => ObjectMapper.Map<TaxRate, TaxRateDto>(x)).ToList());
        }

        public async Task<List<TaxRateDto>> GetAllActiveTaxRatesAsync()
        {
            var items = await _taxRateRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<TaxRate, TaxRateDto>(x)).ToList();
        }

        // PaymentMethod
        public async Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodDto input)
        {
            var entity = new PaymentMethod(Guid.NewGuid(), input.Name, (PaymentType)input.Type, input.Description);
            var created = await _paymentMethodRepository.InsertAsync(entity);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(created);
        }

        public async Task<PaymentMethodDto> UpdatePaymentMethodAsync(Guid id, UpdatePaymentMethodDto input)
        {
            var entity = await _paymentMethodRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.Type = (PaymentType)input.Type;
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;
            var updated = await _paymentMethodRepository.UpdateAsync(entity);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(updated);
        }

        public async Task DeletePaymentMethodAsync(Guid id)
        {
            await _paymentMethodRepository.DeleteAsync(id);
        }

        public async Task<PaymentMethodDto> GetPaymentMethodAsync(Guid id)
        {
            var entity = await _paymentMethodRepository.GetAsync(id);
            return ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(entity);
        }

        public async Task<PagedResultDto<PaymentMethodDto>> GetPaymentMethodListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _paymentMethodRepository.CountAsync();
            var items = await _paymentMethodRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<PaymentMethodDto>(totalCount, items.Select(x => ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(x)).ToList());
        }

        public async Task<List<PaymentMethodDto>> GetAllActivePaymentMethodsAsync()
        {
            var items = await _paymentMethodRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<PaymentMethod, PaymentMethodDto>(x)).ToList();
        }

        // Shift
        public async Task<ShiftDto> CreateShiftAsync(CreateShiftDto input)
        {
            var entity = new Shift(Guid.NewGuid(), input.Name, TimeSpan.Parse(input.StartTime), TimeSpan.Parse(input.EndTime), input.Description);
            var created = await _shiftRepository.InsertAsync(entity);
            return ObjectMapper.Map<Shift, ShiftDto>(created);
        }

        public async Task<ShiftDto> UpdateShiftAsync(Guid id, UpdateShiftDto input)
        {
            var entity = await _shiftRepository.GetAsync(id);
            entity.Name = input.Name;
            entity.StartTime = TimeSpan.Parse(input.StartTime);
            entity.EndTime = TimeSpan.Parse(input.EndTime);
            entity.Description = input.Description;
            entity.IsActive = input.IsActive;
            var updated = await _shiftRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Shift, ShiftDto>(updated);
        }

        public async Task DeleteShiftAsync(Guid id)
        {
            await _shiftRepository.DeleteAsync(id);
        }

        public async Task<ShiftDto> GetShiftAsync(Guid id)
        {
            var entity = await _shiftRepository.GetAsync(id);
            return ObjectMapper.Map<Shift, ShiftDto>(entity);
        }

        public async Task<PagedResultDto<ShiftDto>> GetShiftListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _shiftRepository.CountAsync();
            var items = await _shiftRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
            return new PagedResultDto<ShiftDto>(totalCount, items.Select(x => ObjectMapper.Map<Shift, ShiftDto>(x)).ToList());
        }

        public async Task<List<ShiftDto>> GetAllActiveShiftsAsync()
        {
            var items = await _shiftRepository.GetListAsync(x => x.IsActive);
            return items.Select(x => ObjectMapper.Map<Shift, ShiftDto>(x)).ToList();
        }
    }
}
