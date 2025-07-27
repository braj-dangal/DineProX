using DineProX.Entities.CustomerManagement;
using DineProX.Entities.InventoryManagement;
using DineProX.Entities.Notification;
using DineProX.Entities.PaymentManagement;
using DineProX.Entities.RoleManagement;
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

   
    //Role,Notification,Customer,Payment,Inventory
    public DbSet<RoleExtension> RoleExtensions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Due> Dues { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
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

        builder.Entity<Customer>(b =>
        {
            b.ToTable("Customers");
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
            b.Property(x => x.Address).HasMaxLength(500).IsRequired();
            b.Property(x => x.UserId).IsRequired(false);
        });

        builder.Entity<Payment>(b =>
        {
            b.ToTable("Payments");
            b.ConfigureByConvention();
            b.Property(x => x.OrderId).IsRequired();
            b.Property(x => x.CustomerId).IsRequired();
            b.Property(x => x.AmountPaid).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.Discount).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.TotalBill).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.Date).IsRequired();
        });

        builder.Entity<Due>(b =>
        {
            b.ToTable("Dues");
            b.ConfigureByConvention();
            b.Property(x => x.PaymentId).IsRequired();
            b.Property(x => x.CustomerId).IsRequired();
            b.Property(x => x.TotalAmount).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.AmountPaid).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.RemainingDue).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.DueDate).IsRequired();
            b.Property(x => x.IsSettled).IsRequired();
        });

        builder.Entity<Purchase>(b =>
        {
            b.ToTable("Purchases");
            b.ConfigureByConvention();
            b.Property(x => x.DishId).IsRequired();
            b.Property(x => x.Quantity).IsRequired();
            b.Property(x => x.PurchasePrice).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.SupplierName).HasMaxLength(100).IsRequired();
            b.Property(x => x.PurchaseDate).IsRequired();
        });

        builder.Entity<Inventory>(b =>
        {
            b.ToTable("Inventories");
            b.ConfigureByConvention();
            b.Property(x => x.DishId).IsRequired();
            b.Property(x => x.QuantityAvailable).IsRequired();
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
