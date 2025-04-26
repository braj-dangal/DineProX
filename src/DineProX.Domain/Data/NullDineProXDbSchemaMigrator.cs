using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DineProX.Data;

/* This is used if database provider does't define
 * IDineProXDbSchemaMigrator implementation.
 */
public class NullDineProXDbSchemaMigrator : IDineProXDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
