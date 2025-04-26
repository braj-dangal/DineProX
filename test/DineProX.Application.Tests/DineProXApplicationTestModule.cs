using Volo.Abp.Modularity;

namespace DineProX;

[DependsOn(
    typeof(DineProXApplicationModule),
    typeof(DineProXDomainTestModule)
)]
public class DineProXApplicationTestModule : AbpModule
{

}
