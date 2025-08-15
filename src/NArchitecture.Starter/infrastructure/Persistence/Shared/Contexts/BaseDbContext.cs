using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.EntityFramework.Extensions;

namespace NArchitecture.Starter.Persistence.Shared.Contexts;

public partial class BaseDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssemblyWithDefaultConstructorsOnly(Assembly.GetExecutingAssembly());

        _ = modelBuilder.ApplyTimestampsConfiguration();
        _ = modelBuilder.ApplySoftDeleteConfiguration();
        _ = modelBuilder.ApplyOptimisticConcurrencyConfiguration();
    }
}
