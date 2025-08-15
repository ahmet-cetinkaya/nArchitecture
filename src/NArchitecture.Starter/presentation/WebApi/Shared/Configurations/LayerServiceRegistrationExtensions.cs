using NArchitecture.Starter.Application;
using NArchitecture.Starter.Persistence;
using NArchitecture.Starter.Persistence.Shared.Models;

namespace NArchitecture.Starter.WebApi.Shared.Configurations;

/// <summary>
/// Contains extension methods for registering layer services to the service collection.
/// </summary>
public static class LayerServiceRegistrationExtensions
{
    /// <summary>
    /// Registers application layer services with their configurations.
    /// </summary>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddApplicationServices();

        return services;
    }

    /// <summary>
    /// Registers persistence layer services with their configurations.
    /// </summary>
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        PersistenceConfiguration persistenceConfiguration = new(Database: configuration.GetDatabaseConfiguration());
        _ = services.AddPersistenceServices(persistenceConfiguration);

        return services;
    }
}
