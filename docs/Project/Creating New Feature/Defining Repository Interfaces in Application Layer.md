[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Defining Repository Interfaces in Application Layer`

# 🔌 Defining Repository Interfaces in Application Layer

After defining your domain entities, the next step is to create repository interfaces in the Application layer. These interfaces define the contract that persistence implementations must follow.

## 📋 Steps to Define Repository Interfaces

### 1. Create the appropriate folder structure:

```
Application/
├── Features/
│   └── Inventory/
│       └── Repositories/
│           └── IProductRepository.cs
```

### 2. Create the Repository Interface

Create an interface for your repository in the Application layer:

```csharp
// In Application/Features/Inventory/Repositories/IProductRepository.cs
using NArchitecture.Core.Persistence.Abstractions.Repositories;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Application.Features.Inventory.Repositories;

public interface IProductRepository :
    IAsyncRepository<Product, int>,
    IRepository<Product, int>
{
    // Base repository interfaces provide standard CRUD operations
}
```

### 3. Add Custom Repository Methods (Optional)

If your feature requires specific data access operations, define them in your repository interface:

```csharp
public interface IProductRepository :
    IAsyncRepository<Product, int>,
    IRepository<Product, int>
{
    Task<List<Product>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken);
}
```

## 🔮 Best Practices for Repository Interfaces

1. **Keep Interfaces Focused**: Each repository should be dedicated to a single entity.

2. **Use Domain Language**: Name methods using business terms:

   ```csharp
   Task<List<Product>> GetPopularProductsAsync(CancellationToken cancellationToken);
   ```

3. **Define Asynchronous Methods**: Always include async methods:

   ```csharp
   Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
   ```

4. **Include Cancellation Tokens**: Allow operations to be canceled.

By following these best practices, you can create clean and maintainable repository interfaces that are easy to implement in the persistence layer.
