using Volo.Abp.Modularity;

namespace DineProX;

public abstract class DineProXApplicationTestBase<TStartupModule> : DineProXTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
