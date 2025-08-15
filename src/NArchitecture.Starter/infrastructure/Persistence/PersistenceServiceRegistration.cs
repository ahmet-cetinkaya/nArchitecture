using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Persistence.EntityFramework.DependencyInjection;
using NArchitecture.Starter.Persistence.Shared.Contexts;
using NArchitecture.Starter.Persistence.Shared.Models;

namespace NArchitecture.Starter.Persistence;

public static class PersistenceServiceRegistration
{
    /// <summary>
    /// Adds the persistence services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The persistence configuration.</param>
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        PersistenceConfiguration configuration
    )
    {
        return services.AddDatabase(configuration);
    }

    /// <summary>
    /// Configures and adds the database context
    /// </summary>
    private static IServiceCollection AddDatabase(this IServiceCollection services, PersistenceConfiguration configuration)
    {
        // Setup for in-memory database
        _ = services.AddDbContext<BaseDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: configuration.Database.Name)
        );

        // Add the database migration applier
        _ = services.AddDbMigrationApplier(sp =>
            sp.GetService<BaseDbContext>() ?? throw new InvalidOperationException("Database context not found.")
        );

        return services;
    }
}
