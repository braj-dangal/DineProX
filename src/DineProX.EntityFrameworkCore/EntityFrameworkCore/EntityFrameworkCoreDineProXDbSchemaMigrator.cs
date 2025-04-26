using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DineProX.Data;
using Volo.Abp.DependencyInjection;

namespace DineProX.EntityFrameworkCore;

public class EntityFrameworkCoreDineProXDbSchemaMigrator
    : IDineProXDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreDineProXDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the DineProXDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<DineProXDbContext>()
            .Database
            .MigrateAsync();
    }
}
