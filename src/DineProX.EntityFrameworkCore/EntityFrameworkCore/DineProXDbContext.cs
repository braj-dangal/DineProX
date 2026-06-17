using DineProX.Entities.Notification;
using DineProX.Entities.RoleManagement;
using DineProX.Entities.MasterData;
﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace DineProX.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class DineProXDbContext :
    AbpDbContext<DineProXDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityUserRole> UserRoles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

   
    //Role, Notification
    public DbSet<RoleExtension> RoleExtensions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    //Master Data
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<TableZone> TableZones { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<TaxRate> TaxRates { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DineProXDbContext(DbContextOptions<DineProXDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Notification>(b =>
        {
            b.ToTable("Notifications");
            b.ConfigureByConvention();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.ReceiverId).IsRequired();
            b.Property(x => x.Template).HasMaxLength(5000).IsRequired();
        });

        builder.Entity<RoleExtension>(b =>
        {
            b.ToTable("RoleExtensions");
            b.ConfigureByConvention();
            b.HasOne<IdentityRole>().WithMany().HasForeignKey(x => x.AbpRoleId).IsRequired();
            b.Property(x => x.AbpRoleName).HasMaxLength(50).IsRequired();
            b.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
            b.Property(x => x.IsActive).IsRequired(false);
        });

        /* Master Data Entity Configurations */
        builder.Entity<ItemCategory>(b =>
        {
            b.ToTable("MasterData_ItemCategories");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.DisplayOrder).IsRequired();
            b.Property(x => x.IsActive).IsRequired();
        });

        builder.Entity<MenuItem>(b =>
        {
            b.ToTable("MasterData_MenuItems");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.StockUnit).IsRequired().HasMaxLength(50);
            b.Property(x => x.Price).HasPrecision(18, 2);
            b.Property(x => x.TaxPercentage).HasPrecision(5, 2);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.Allergens).HasMaxLength(500);
            b.Property(x => x.IsActive).IsRequired();
            b.HasOne<ItemCategory>().WithMany().HasForeignKey(x => x.CategoryId).IsRequired();
        });

        builder.Entity<Table>(b =>
        {
            b.ToTable("MasterData_Tables");
            b.ConfigureByConvention();
            b.Property(x => x.TableNumber).IsRequired().HasMaxLength(50);
            b.Property(x => x.Capacity).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.IsActive).IsRequired();
            b.HasOne<TableZone>().WithMany().HasForeignKey(x => x.ZoneId).IsRequired(false);
        });

        builder.Entity<TableZone>(b =>
        {
            b.ToTable("MasterData_TableZones");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.IsActive).IsRequired();
        });

        builder.Entity<Supplier>(b =>
        {
            b.ToTable("MasterData_Suppliers");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.ContactPerson).HasMaxLength(128);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(20);
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.City).HasMaxLength(128);
            b.Property(x => x.PostalCode).HasMaxLength(20);
            b.Property(x => x.Country).HasMaxLength(128);
            b.Property(x => x.PaymentTerms).HasMaxLength(256);
            b.Property(x => x.CreditLimit).HasPrecision(18, 2);
            b.Property(x => x.IsActive).IsRequired();
        });

        builder.Entity<TaxRate>(b =>
        {
            b.ToTable("MasterData_TaxRates");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Rate).HasPrecision(5, 2);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.IsActive).IsRequired();
        });

        builder.Entity<PaymentMethod>(b =>
        {
            b.ToTable("MasterData_PaymentMethods");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.IsActive).IsRequired();
        });

        builder.Entity<Shift>(b =>
        {
            b.ToTable("MasterData_Shifts");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.StartTime).IsRequired();
            b.Property(x => x.EndTime).IsRequired();
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.IsActive).IsRequired();
        });

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(DineProXConsts.DbTablePrefix + "YourEntities", DineProXConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
