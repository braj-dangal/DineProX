using Volo.Abp.Modularity;

namespace DineProX;

/* Inherit from this class for your domain layer tests. */
public abstract class DineProXDomainTestBase<TStartupModule> : DineProXTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
