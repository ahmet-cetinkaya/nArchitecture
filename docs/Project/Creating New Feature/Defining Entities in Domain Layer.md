[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Defining Entities in Domain Layer`

# 📦 Defining Entities in Domain Layer

Entities represent the core business objects in your domain layer. This document will guide you through defining entities in NArchitecture.

## 📝 Steps to Define an Entity

1. Create an appropriate folder structure:

   ```
   Domain/
   ├── Features/
   │   └── Inventory/         # Feature name reflects business domain
   │       └── Entities/
   │           └── Product.cs
   ```

2. Create your entity class inheriting from the appropriate base class

   ```csharp
   using NArchitecture.Core.Persistence.Abstractions.Repositories;

   namespace NArchitecture.Starter.Domain.Features.Inventory.Entities;

   public class Product : BaseEntity<int>
   {
       public string Name { get; set; }
       public decimal Price { get; set; }
       public int Stock { get; set; }

       [Obsolete("This constructor is for ORM, mapper etc.. Do not use it in the code.", true)]
       public Product()
       {
           // Required for EF Core, serialization, etc.
       }

       public Product(string name, decimal price, int stock)
       {
           Name = name;
           Price = price;
           Stock = stock;
       }
   }
   ```

## 🔮 Entity Design Best Practices

1. **Mark Parameterless Constructor with Obsolete**: This communicates that the constructor is only for frameworks, not for direct use:

   ```csharp
   [Obsolete("This constructor is for ORM, mapper etc.. Do not use it in the code.", true)]
   public Product()
   {
   }
   ```

2. **Provide Business Constructor**: Create constructors that ensure the entity is created in a valid state:

   ```csharp
   public Product(string name, decimal price)
   {
       Name = name;
       Price = price;
       Stock = 0; // Default value
   }
   ```

3. **Use Appropriate Data Types**: Choose the correct data types for your properties:
   ```csharp
   public string SKU { get; set; } // For product code
   public DateTime ReleaseDate { get; set; } // For product launch date
   public bool IsActive { get; set; } // For product availability status
   ```

By following these best practices, you can create entities that are easy to maintain and understand.

### 📚 Base Entity Properties

All entities automatically include these properties from `BaseEntity<TId>`:

- `Id`: Unique identifier of the entity
- `CreatedAt`: Creation timestamp
- `UpdatedAt`: Last update timestamp
- `DeletedAt`: Soft delete timestamp
- `RowVersion`: Version information for concurrency control
