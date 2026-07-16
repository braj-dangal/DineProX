# DineProX Master Data - Database Setup Guide

## Overview
This guide explains how to set up the Master Data entities in the DineProX database using Entity Framework Core migrations.

## Master Data Entities

The following entities have been created for Master Data management:

1. **ItemCategory** - Menu item categories (Beverages, Appetizers, etc.)
2. **MenuItem** - Menu items with pricing, stock, and allergen info
3. **Table** - Dine-in tables with capacity and zone assignment
4. **TableZone** - Zones/sections within restaurant (optional)
5. **Supplier** - Vendor information for inventory management
6. **TaxRate** - Tax configuration for sales
7. **PaymentMethod** - Payment options (Cash, Card, Wallet, etc.)
8. **Shift** - Employee work shifts for HRMS

## Steps to Create Database Migration

### 1. Update DbContext
Add the Master Data entities to the DineProX DbContext in `DineProX.EntityFrameworkCore`:

```csharp
public class DineProXDbContext : AbpDbContext<DineProXDbContext>
{
    // ... existing DbSets ...
    
    // Master Data DbSets
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<TableZone> TableZones { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Shift> Shifts { get; set; }
}
```

### 2. Configure Entity Mappings
In the `OnModelCreating` method, add fluent API configurations:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // Master Data Configurations
    builder.Entity<ItemCategory>(b =>
    {
        b.ToTable("MasterData_ItemCategories");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        b.Property(x => x.Description).HasMaxLength(500);
    });

    builder.Entity<MenuItem>(b =>
    {
        b.ToTable("MasterData_MenuItems");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        b.Property(x => x.StockUnit).IsRequired().HasMaxLength(50);
        b.Property(x => x.Price).HasPrecision(18, 2);
        b.Property(x => x.TaxPercentage).HasPrecision(5, 2);
    });

    builder.Entity<Table>(b =>
    {
        b.ToTable("MasterData_Tables");
        b.ConfigureByConvention();
        b.Property(x => x.TableNumber).IsRequired().HasMaxLength(50);
    });

    builder.Entity<TableZone>(b =>
    {
        b.ToTable("MasterData_TableZones");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    });

    builder.Entity<Supplier>(b =>
    {
        b.ToTable("MasterData_Suppliers");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(256);
        b.Property(x => x.Email).HasMaxLength(256);
        b.Property(x => x.Phone).HasMaxLength(20);
        b.Property(x => x.CreditLimit).HasPrecision(18, 2);
    });

    builder.Entity<TaxRate>(b =>
    {
        b.ToTable("MasterData_TaxRates");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        b.Property(x => x.Rate).HasPrecision(5, 2);
    });

    builder.Entity<PaymentMethod>(b =>
    {
        b.ToTable("MasterData_PaymentMethods");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    });

    builder.Entity<Shift>(b =>
    {
        b.ToTable("MasterData_Shifts");
        b.ConfigureByConvention();
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    });
}
```

### 3. Create Migration
From the package manager console in Visual Studio or terminal:

```bash
# Navigate to the EntityFrameworkCore project directory
cd src/DineProX.EntityFrameworkCore

# Add new migration
dotnet ef migrations add AddMasterDataEntities
```

### 4. Update Database
Apply the migration to the database:

```bash
# From the root project directory
dotnet ef database update --project src/DineProX.EntityFrameworkCore
```

Or from Package Manager Console:
```
Update-Database
```

## Data Seeding (Optional)

To seed initial Master Data, create a data contributor in `DineProX.Domain/DataSeedContributor`:

```csharp
public class MasterDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<ItemCategory, Guid> _itemCategoryRepository;
    private readonly IRepository<PaymentMethod, Guid> _paymentMethodRepository;
    // ... other repositories ...

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _itemCategoryRepository.CountAsync() > 0)
        {
            return;
        }

        // Seed Item Categories
        await _itemCategoryRepository.InsertManyAsync(new List<ItemCategory>
        {
            new ItemCategory(Guid.NewGuid(), "Beverages", "All drinks", 1),
            new ItemCategory(Guid.NewGuid(), "Appetizers", "Starter items", 2),
            new ItemCategory(Guid.NewGuid(), "Main Course", "Main dishes", 3),
            new ItemCategory(Guid.NewGuid(), "Desserts", "Sweet items", 4),
        });

        // Seed Payment Methods
        await _paymentMethodRepository.InsertManyAsync(new List<PaymentMethod>
        {
            new PaymentMethod(Guid.NewGuid(), "Cash", PaymentType.Cash),
            new PaymentMethod(Guid.NewGuid(), "Card", PaymentType.Card),
            new PaymentMethod(Guid.NewGuid(), "Wallet", PaymentType.Wallet),
        });

        // Add more seed data as needed
    }
}
```

## API Endpoints

### ItemCategory
- `GET /api/itemcategory` - List all categories
- `GET /api/itemcategory/{id}` - Get category details
- `GET /api/itemcategory/all-active` - Get active categories
- `POST /api/itemcategory` - Create new category
- `PUT /api/itemcategory/{id}` - Update category
- `DELETE /api/itemcategory/{id}` - Delete category

### MenuItem
- `GET /api/menuitem` - List all items
- `GET /api/menuitem/{id}` - Get item details
- `GET /api/menuitem/by-category/{categoryId}` - Items by category
- `GET /api/menuitem/low-stock` - Items below reorder level
- `POST /api/menuitem` - Create new item
- `PUT /api/menuitem/{id}` - Update item
- `PATCH /api/menuitem/{id}/adjust-stock?quantity=10` - Adjust stock
- `DELETE /api/menuitem/{id}` - Delete item

### Table
- `GET /api/table` - List all tables
- `GET /api/table/{id}` - Get table details
- `GET /api/table/by-zone/{zoneId}` - Tables in a zone
- `GET /api/table/by-status/{status}` - Tables by status (0=Free, 1=Occupied, 2=Reserved)
- `POST /api/table` - Create new table
- `PUT /api/table/{id}` - Update table
- `PATCH /api/table/{id}/mark-occupied` - Mark as occupied
- `PATCH /api/table/{id}/mark-free` - Mark as free
- `PATCH /api/table/{id}/mark-reserved` - Mark as reserved
- `DELETE /api/table/{id}` - Delete table

### TableZone
- `GET /api/tablezone` - List all zones
- `GET /api/tablezone/{id}` - Get zone details
- `GET /api/tablezone/all-active` - Active zones only
- `POST /api/tablezone` - Create new zone
- `PUT /api/tablezone/{id}` - Update zone
- `DELETE /api/tablezone/{id}` - Delete zone

### Supplier
- `GET /api/supplier` - List all suppliers
- `GET /api/supplier/{id}` - Get supplier details
- `GET /api/supplier/all-active` - Active suppliers only
- `GET /api/supplier/search?name=xyz` - Search suppliers by name
- `POST /api/supplier` - Create new supplier
- `PUT /api/supplier/{id}` - Update supplier
- `DELETE /api/supplier/{id}` - Delete supplier

### TaxRate
- `GET /api/taxrate` - List all tax rates
- `GET /api/taxrate/{id}` - Get tax rate details
- `GET /api/taxrate/all-active` - Active rates only
- `POST /api/taxrate` - Create new tax rate
- `PUT /api/taxrate/{id}` - Update tax rate
- `DELETE /api/taxrate/{id}` - Delete tax rate

### PaymentMethod
- `GET /api/paymentmethod` - List all payment methods
- `GET /api/paymentmethod/{id}` - Get method details
- `GET /api/paymentmethod/all-active` - Active methods only
- `POST /api/paymentmethod` - Create new method
- `PUT /api/paymentmethod/{id}` - Update method
- `DELETE /api/paymentmethod/{id}` - Delete method

### Shift
- `GET /api/shift` - List all shifts
- `GET /api/shift/{id}` - Get shift details
- `GET /api/shift/all-active` - Active shifts only
- `POST /api/shift` - Create new shift
- `PUT /api/shift/{id}` - Update shift
- `DELETE /api/shift/{id}` - Delete shift

## Validation Notes

- All entity names are required and have maximum length constraints
- Prices and rates use decimal precision (18,2) for financial accuracy
- Foreign key relationships:
  - MenuItem → ItemCategory
  - Table → TableZone (optional)
- Enum Types:
  - `TableStatus`: Free (0), Occupied (1), Reserved (2)
  - `PaymentType`: Cash (0), Card (1), Wallet (2), Cheque (3), BankTransfer (4)

## Next Steps

1. ✅ Implement DbContext integration
2. ✅ Create and apply migrations
3. ✅ Seed initial Master Data
4. ⏭️ Implement validation rules (required)
5. ⏭️ Add permission checks to endpoints
6. ⏭️ Implement audit logging
7. ⏭️ Create frontend interfaces for Master Data management

