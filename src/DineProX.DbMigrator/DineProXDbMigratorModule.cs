using DineProX.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DineProX.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DineProXEntityFrameworkCoreModule),
    typeof(DineProXApplicationContractsModule)
    )]
public class DineProXDbMigratorModule : AbpModule
{
}
