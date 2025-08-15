using NArchitecture.Starter.Persistence.Shared.Models;

namespace NArchitecture.Starter.WebApi.Shared.Configurations;

public static class DatabaseConfigurationExtensions
{
    /// <summary>
    /// Gets the database configuration from the configuration.
    /// </summary>
    public static DatabaseConfiguration GetDatabaseConfiguration(this IConfiguration configuration)
    {
        DatabaseConfiguration databaseConfiguration = configuration.GetSection("Database").Get<DatabaseConfiguration>();
        if (string.IsNullOrWhiteSpace(databaseConfiguration.Name))
            throw new InvalidOperationException("Database:Name cannot be empty.");

        return databaseConfiguration;
    }
}
