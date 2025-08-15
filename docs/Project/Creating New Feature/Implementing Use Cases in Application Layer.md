[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Implementing Use Cases in Application Layer`

# 📝 Implementing Use Cases in Application Layer

After defining your domain entities and repository interfaces, the next step is to implement the use cases in the Application layer. NArchitecture uses the CQRS (Command Query Responsibility Segregation) pattern to organize use cases into commands and queries.

## 🔄 Understanding CQRS in NArchitecture

CQRS separates operations that read data (queries) from operations that change data (commands):

- **Commands**: Modify state and represent actions (create, update, delete)
- **Queries**: Return data without modifying state

## 📋 Steps to Implement Use Cases

### 1. Create the appropriate folder structure:

```
Application/
├── Features/
│   └── Inventory/
│       ├── Commands/
│       │   └── Create/
│       │       ├── CreateProductCommand.cs
│       │       ├── CreateProductCommandHandler.cs
│       │       ├── CreatedProductResponse.cs
│       └── Queries/
│           └── GetById/
│               ├── GetProductByIdQuery.cs
│               ├── GetProductByIdQueryHandler.cs
│               ├── GetProductByIdResponse.cs
```

### 2. Create the Command/Query

Define the command or query record with its parameters:

```csharp
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Mediator.Abstractions.CQRS;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity
) : ICommand<CreatedProductResponse>, ILoggableRequest;
```

### 3. Implement the Response

Create a response model representing the operation result:

```csharp
namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public record CreatedProductResponse(
    int Id,
    string Name,
    decimal Price,
    DateTime CreatedAt
);
```

### 4. Implement the Handler

Create a handler that processes the command/query:

```csharp
using NArchitecture.Core.Mediator.Abstractions.CQRS;
using NArchitecture.Starter.Application.Features.Inventory.Repositories;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductCommandHandler(IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand, CreatedProductResponse>
{
    public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Create entity from command
        Product product = new()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        // Save entity
        await productRepository.AddAsync(product, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);

        // Create response from entity
        CreatedProductResponse response = new(
            product.Id,
            product.Name,
            product.Price,
            product.CreatedAt
        );

        return response;
    }
}
```

### Registering CQRS Handlers

Command and query handlers are automatically discovered and registered using the `AddMediator()` method in your `ApplicationServiceRegistration` class:

```csharp
// Application/ApplicationServiceRegistration.cs

public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // Register NArchitecture.Core.Mediator.Abstractions.IMediator service
    services.AddMediator();

    ...

    return services;
}
```

This method registers the implementation of `IMediator` which is used to send commands and queries to their respective handlers.

## 📚 Business Rules

For complex business logic, create business rules in separate classes:

```csharp
// Application/Features/Inventory/Rules/ProductBusinessRules.cs

using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Starter.Application.Features.Inventory.Repositories;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Application.Features.Inventory.Rules;

public class ProductBusinessRules(IProductRepository productRepository)
{
    public async Task ProductNameShouldBeUniqueAsync(string name, CancellationToken cancellationToken)
    {
        bool exists = await productRepository.AnyAsync(
            predicate: p => p.Name == name,
            cancellationToken: cancellationToken
        );

        if (exists)
            throw new BusinessException("Product with this name already exists.");
    }

    public void ProductShouldExist(Product? product)
    {
        if (product is null)
            throw new BusinessException("Product not found.");
    }
}
```

### Registering Business Rules

Business rules are registered automatically with the `AddBusinessRules()` method in your `ApplicationServiceRegistration` class:

```csharp
// Application/ApplicationServiceRegistration.cs

public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // Register NArchitecture.Core.Application.IBusinessRules services
    services.AddBusinessRules();

    ...

    return services;
}
```

This registration scans all classes implementing `IBusinessRules` and registers them with the dependency injection container.

Using business rules in a handler:

```csharp
using NArchitecture.Starter.Application.Features.Inventory.Rules;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductCommandHandler(IProductRepository productRepository, ProductBusinessRules productBusinessRules)
    : ICommandHandler<CreateProductCommand, CreatedProductResponse>
{
    public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Apply business rules
        await productBusinessRules.ProductNameMustBeUniqueAsync(request.Name, cancellationToken);

        ...
    }
}
```

## ✅ Validation (Optional but Recommended)

Implement validation using FluentValidation to ensure input quality:

```csharp
// Application/Features/Inventory/Commands/Create/CreateProductCommandValidator.cs

using FluentValidation;
using NArchitecture.Core.Validation.Abstractions;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>, IValidationProfile<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");
    }
}
```

### Registering Validators

Validators are registered automatically with the `AddFluentValidation()` method in your `ApplicationServiceRegistration` class:

```csharp
// Application/ApplicationServiceRegistration.cs

public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // Register NArchitecture.Core.Validation.Abstractions.IValidator<T> services
    // for all FluentValidation.IValidator<T> services
    services.AddFluentValidation(assemblies: [typeof(ApplicationServiceRegistration).Assembly]);

    ...

    return services;
}
```

This registers all implementations of FluentValidation's `IValidator<T>` as `NArchitecture.Core.Validation.Abstractions.IValidator<T>` services.

When a validator class is defined for a command or query, NArchitecture's validation pipeline behavior will automatically validate the request before it reaches the handler. If validation fails, the handler won't be executed, and a validation exception will be thrown.

## 🔄 Object Mapping (Optional)

Create mapping profiles to handle object transformations between layers using Mapster:

```csharp
// Application/Features/Inventory/Commands/Create/CreateProductMappingProfile.cs

using Mapster;
using NArchitecture.Core.Mapper.Abstractions;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductMappingProfile : IRegister, IMappingProfile<CreateProductMappingProfile>
{
    public void Register(TypeAdapterConfig config)
    {
        // Command to entity mapping
        config.NewConfig<CreateProductCommand, Product>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.DeletedAt);

        // Entity to response mapping
        config.NewConfig<Product, CreatedProductResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);
    }
}
```

### Registering Mapping Profiles

Mapping profiles are registered with the `AddNArchitectureMapster()` method in your `ApplicationServiceRegistration` class:

```csharp
// Application/ApplicationServiceRegistration.cs

public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // Register NArchitecture.Core.Mapper.Abstractions.IMapper services
    // for all Mapster mapping profiles
    services.AddNArchitectureMapster(typeof(ApplicationServiceRegistration).Assembly);

    ...

    return services;
}
```

This registers Mapster profiles and provides an implementation of `NArchitecture.Core.Mapper.Abstractions.IMapper`.

### Advanced Registration Options

You can also use additional configuration options:

```csharp
// With custom configuration
services.AddNArchitectureMapster(
    configAction: config => {
        // Global Mapster configuration
        config.Default.PreserveReference(true);
    },
    typeof(ApplicationServiceRegistration).Assembly
);

// With different service lifetime
services.AddNArchitectureMapster(
    ServiceLifetime.Scoped,
    typeof(ApplicationServiceRegistration).Assembly
);

// Using global configuration
services.AddNArchitectureMapsterGlobal(
    configAction: config => {
        // Configure global TypeAdapterConfig
        config.Default.PreserveReference(true);
    }
);
```

When using object mapping in your handlers:

```csharp
using NArchitecture.Core.Mapper.Abstractions;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    : ICommandHandler<CreateProductCommand, CreatedProductResponse>
{
    public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Create entity from command
        Product product = mapper.Map<Product>(request);

        ...

        // Create response from entity
        var response = mapper.Map<CreatedProductResponse>(product);
        return response;
    }
}
```

## 🌐 Localization (Optional)

For internationalization support, implement localization for your feature:

### 1. Create Constants for Message Keys

Define message keys in a constants file:

```csharp
namespace NArchitecture.Starter.Application.Features.Inventory.Constants;

public static class ProductMessages
{
    public static class General
    {
        public const string NotFound = "product.not_found";
        public const string AlreadyExists = "product.already_exists";
    }

    public static class Validation
    {
        public const string NameRequired = "product.validation.name_required";
        public const string PricePositive = "product.validation.price_positive";
        public const string StockNonNegative = "product.validation.stock_non_negative";
    }
}
```

### 2. Create Localization Resource Files

Create YAML files for each supported language:

```
Application/
├── Features/
│   └── Inventory/
│       └── Resources/
│           └── Locales/
│               ├── Product.en.yaml
│               ├── Product.tr.yaml
│               └── Product.fr.yaml
```

#### English Localization Example (Product.en.yaml)

```yaml
product:
  not_found: Product could not be found.
  already_exists: A product with this name already exists.
  validation:
    name_required: Product name is required.
    price_positive: Price must be greater than zero.
    stock_non_negative: Stock quantity cannot be negative.
```

#### Turkish Localization Example (Product.tr.yaml)

```yaml
product:
  not_found: Ürün bulunamadı.
  already_exists: Bu isimde bir ürün zaten mevcut.
  validation:
    name_required: Ürün adı gereklidir.
    price_positive: Fiyat sıfırdan büyük olmalıdır.
    stock_non_negative: Stok miktarı negatif olamaz.
```

### 3. Use Localized Messages in Validators

```csharp
using FluentValidation;
using NArchitecture.Core.Localization.Abstractions;
using NArchitecture.Core.Validation.Abstractions;
using NArchitecture.Starter.Application.Features.Inventory.Constants;

namespace NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

public class CreateProductCommandValidator(ILocalizationService localizationService)
    : AbstractValidator<CreateProductCommand>, IValidationProfile<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(async _ =>
                await localizationService.GetLocalizedAsync(ProductMessages.Validation.NameRequired));

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage(async _ =>
                await localizationService.GetLocalizedAsync(ProductMessages.Validation.PricePositive));

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage(async _ =>
                await localizationService.GetLocalizedAsync(ProductMessages.Validation.StockNonNegative));
    }
}
```

### 4. Use Localized Messages in Business Rules

```csharp
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstractions;
using NArchitecture.Starter.Application.Features.Inventory.Constants;
using NArchitecture.Starter.Application.Features.Inventory.Repositories;
using NArchitecture.Starter.Domain.Features.Inventory.Entities;

namespace NArchitecture.Starter.Application.Features.Inventory.Rules;

public class ProductBusinessRules(
    IProductRepository productRepository,
    ILocalizationService localizationService)
{
    public async Task ProductShouldExistAsync(Product? product, CancellationToken cancellationToken)
    {
        if (product is null)
        {
            string localizedMessage = await localizationService.GetLocalizedAsync(
                ProductMessages.General.NotFound,
                cancellationToken: cancellationToken
            );
            throw new BusinessException(localizedMessage);
        }
    }

    public async Task ProductNameShouldBeUniqueAsync(string name, CancellationToken cancellationToken)
    {
        bool exists = await productRepository.AnyAsync(
            predicate: p => p.Name == name,
            cancellationToken: cancellationToken
        );

        if (exists)
        {
            string localizedMessage = await localizationService.GetLocalizedAsync(
                ProductMessages.General.AlreadyExists,
                cancellationToken: cancellationToken
            );
            throw new BusinessException(localizedMessage);
        }
    }
}
```

### Registering Localization Services

For YAML-based localization used in NArchitecture, register the services in your `ApplicationServiceRegistration` class:

```csharp
// Application/ApplicationServiceRegistration.cs
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // Register NArchitecture.Core.Localization.Abstractions.ILocalizationService service
    // with YamlResourceLocalization
    services.AddYamlResourceLocalization();

    ...

    return services;
}
```

This registers an implementation of `ILocalizationService` that uses YAML files for retrieving localized messages.

## 🔮 Best Practices for Use Case Implementation

1. **Keep Commands and Queries Separate**: Maintain a clear distinction between state-changing and read-only operations.

2. **Use Pipeline Behaviors**: Leverage NArchitecture's pipeline behaviors:

   ```csharp
   public record CreateProductCommand(...) :
       ICommand<CreatedProductResponse>,
       ILoggableRequest,             // Enables logging
       ITransactionalRequest,        // Wraps in a transaction
       ICacheRemoverRequest,         // Removes related cache entries
       ISecuredRequest;              // Enforces authorization
   ```

3. **Validate Input**: Always implement validators for commands and complex queries.

4. **Use IMapper for Object Mapping**: Never expose domain entities directly; use IMapper to transform entities into DTOs:

   ```csharp
   // In your handler:
   return mapper.Map<ProductDetailResponse>(product);
   ```

5. **Use Business Rules**: Extract complex validation into business rule classes.

6. **Implement Proper Error Handling**: Use exceptions for business rule violations:
   ```csharp
   if (product is null)
       throw new BusinessException("Product not found.");
   ```

By following these patterns, you'll create maintainable, testable use cases that clearly represent your application's behavior.
