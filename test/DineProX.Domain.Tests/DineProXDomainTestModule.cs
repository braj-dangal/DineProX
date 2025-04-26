using Volo.Abp.Modularity;

namespace DineProX;

[DependsOn(
    typeof(DineProXDomainModule),
    typeof(DineProXTestBaseModule)
)]
public class DineProXDomainTestModule : AbpModule
{

}
