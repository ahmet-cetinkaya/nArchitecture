[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Implementing Repositories in Persistence Layer`

# 🔍 Implementing Repositories in Persistence Layer

This document guides you through implementing repositories in the Persistence layer for your features in the NArchitecture project.

## 🚀 Steps to Implement Repositories

### 1. Create the appropriate folder structure:

```
Persistence/
├── Features/
│   └── Inventory/
│       └── Repositories/
│           └── EfProductRepository.cs
```

### 2. Create the Repository Implementation

Create the Entity Framework implementation in the Persistence layer using primary constructors:

```csharp
// In Persistence/Features/Inventory/Repositories/EfProductRepository.cs
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.EntityFramework.Repositories;
using NArchitecture.Starter.Application.Features.Inventory.Repositories;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;
using NArchitecture.Starter.Persistence.Shared.Contexts;

namespace NArchitecture.Starter.Persistence.Features.Inventory.Repositories;

public class EfProductRepository(BaseDbContext context) :
    EfRepositoryBase<Product, int, BaseDbContext>(context),
    IProductRepository
{
    // Base repository methods are already implemented by EfRepositoryBase
}
```

### 3. Implement Custom Repository Methods

If your repository interface defines custom methods, implement them in your repository class:

```csharp
public async Task<List<Product>> GetByNameAsync(string name, CancellationToken cancellationToken)
{
    return await Context.Products
        .Where(p => p.Name.Contains(name) && p.DeletedAt == null)
        .ToListAsync(cancellationToken);
}

public async Task<List<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken)
{
    return await Context.Products
        .Where(p => p.Stock < threshold && p.DeletedAt == null)
        .ToListAsync(cancellationToken);
}
```

### 4. Register the Repository

Register your repository in the DI container through the PersistenceServiceRegistration class:

```csharp
// In PersistenceServiceRegistration.cs
public static IServiceCollection AddPersistenceServices(
    this IServiceCollection services,
    PersistenceConfiguration configuration)
{
    // ...existing code...

    // Register your repository
    services.AddScoped<IProductRepository, EfProductRepository>();

    return services;
}
```

## 🌟 Repository Implementation Best Practices

1. **Optimize Read Queries**: Use AsNoTracking for read-only operations:

   ```csharp
   public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
   {
       return await Context.Products
           .AsNoTracking()
           .Where(p => p.DeletedAt == null)
           .ToListAsync(cancellationToken);
   }
   ```

2. **Use Pagination**: For potentially large result sets:

   ```csharp
   public async Task<List<Product>> GetProductsPagedAsync(
       int page,
       int pageSize,
       CancellationToken cancellationToken)
   {
       return await Context.Products
           .Where(p => p.DeletedAt == null)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync(cancellationToken);
   }
   ```

3. **Implement Simple Filtering**:
   ```csharp
   public async Task<List<Product>> GetProductsByPriceRangeAsync(
       decimal minPrice,
       decimal maxPrice,
       CancellationToken cancellationToken)
   {
       return await Context.Products
           .Where(p => p.Price >= minPrice &&
                    p.Price <= maxPrice &&
                    p.DeletedAt == null)
           .ToListAsync(cancellationToken);
   }
   ```

By following these practices, you'll create efficient and maintainable repository implementations.
