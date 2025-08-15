namespace NArchitecture.Starter.Persistence.Shared.Models;

/// <summary>
/// Configuration for persistence services.
/// </summary>
/// <param name="Database">The database configuration.</param>
public record struct PersistenceConfiguration(DatabaseConfiguration Database);
