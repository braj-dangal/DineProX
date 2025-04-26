using System.Threading.Tasks;

namespace DineProX.Data;

public interface IDineProXDbSchemaMigrator
{
    Task MigrateAsync();
}
