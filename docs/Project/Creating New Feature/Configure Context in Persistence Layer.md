[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Configure Context in Persistence Layer`

# 💾 Configure Context in Persistence Layer

This document guides you through the process of configuring your database context for your new feature in the NArchitecture project.

## 📝 Steps to Configure Context

### 1. Create the appropriate folder structure:

```
Persistence/
├── Features/
│   └── Inventory/
│       ├── Contexts/
│       │   ├── BaseDbContext.Inventory.cs
│       │   └── Configurations/
│       │       └── ProductConfiguration.cs
│       └── Seeds/
│           └── ProductSeeds.cs
```

### 2. Extend BaseDbContext

Add your entity to the BaseDbContext by creating or updating a partial class file:

```csharp
// In Persistence/Features/Inventory/Contexts/BaseDbContext.Inventory.cs
using Microsoft.EntityFrameworkCore;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Persistence.Shared.Contexts;

public partial class BaseDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}
```

### 3. Create Entity Configuration

Define how your entity is mapped to the database:

```csharp
// In Persistence/Features/Inventory/Contexts/Configurations/ProductConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;
using NArchitecture.Starter.Persistence.Features.Inventory.Seeds;

namespace NArchitecture.Starter.Persistence.Features.Inventory.Contexts.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Property configurations
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        // Index for faster searches
        builder.HasIndex(p => p.Name);

        // Seed initial data
        builder.HasData(ProductSeeds.CreateSeeds());
    }
}
```

### 4. Add Seed Data (Optional)

If your feature requires initial data:

```csharp
// In Persistence/Features/Inventory/Seeds/ProductSeeds.cs
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Persistence.Features.Inventory.Seeds;

public static class ProductSeeds
{
    public static class Ids
    {
        public const int Laptop = 1;
        public const int Phone = 2;
    }

    public static Product[] CreateSeeds()
    {
        return
        [
            new Product { Id = Ids.Laptop, Name = "Laptop", Price = 1200.00m, Stock = 10, CreatedAt = DateTime.MinValue },
            new Product { Id = Ids.Phone, Name = "Phone", Price = 800.00m, Stock = 20, CreatedAt = DateTime.MinValue }
        ];
    }
}
```

## 🔮 Context Configuration Best Practices

1. **Define Database Indexes**: Create indexes for frequently searched columns:

   ```csharp
   builder.HasIndex(p => p.Name);
   ```

2. **Configure Relationships**: Define relationships clearly:

   ```csharp
   // If Product has a Category
   builder.HasOne(p => p.Category)
          .WithMany(c => c.Products)
          .HasForeignKey(p => p.CategoryId);
   ```

3. **Use Table and Column Naming**: Set explicit names if needed:
   ```csharp
   builder.ToTable("ProductItems");
   builder.Property(p => p.Name).HasColumnName("ProductName");
   ```

By following these simple practices, you'll create maintainable database configurations for your features.
