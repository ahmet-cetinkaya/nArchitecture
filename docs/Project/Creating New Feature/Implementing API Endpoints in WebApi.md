[`🏠`](../../README.md) > `...` > [`Creating New Feature`](./README.md) > `Implementing API Endpoints in WebApi`

# 🌐 Implementing API Endpoints in WebApi

After implementing use cases in the Application layer, the next step is to expose them through API endpoints in the WebApi project. NArchitecture uses ASP.NET Core's Minimal API approach for creating clean, maintainable endpoints with a clear separation of concerns.

## 🏗️ Understanding the Endpoint Structure

NArchitecture organizes endpoints using a hierarchical structure:

1. **Feature Groups**: Endpoints are organized by feature areas (Auth, Inventory, etc.)
2. **Endpoint Files**: Each operation has its own endpoint file
3. **Registration Classes**: Registration classes wire up endpoints to the WebApplication

```
WebApi/
├── Features/
│   ├── FeatureEndpointRegistration.cs     # Registers all feature groups
│   ├── Inventory/
│   │   ├── InventoryEndpointRegistration.cs  # Registers all inventory endpoints
│   │   └── Endpoints/
│   │       ├── CreateProductEndpoint.cs      # Individual endpoint implementation
│   │       ├── GetAllProductsEndpoint.cs
│   │       └── GetProductByIdEndpoint.cs
```

## 📋 Steps to Implement API Endpoints

### 1. Create the Endpoint File

Create a new file for your endpoint in the appropriate feature folder:

```csharp
using Microsoft.AspNetCore.Http.HttpResults;
using NArchitecture.Core.Mediator.Abstractions;
using NArchitecture.Starter.Application.Features.Inventory.Commands.Create;

namespace NArchitecture.Starter.WebApi.Features.Inventory.Endpoints;

public static class CreateProductEndpoint
{
    public static RouteGroupBuilder MapCreateProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .WithName("CreateProduct")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Create a new product";
                operation.Description = "Creates a new product with the given details";
                return operation;
            });

        return group;
    }

    // Request DTO to match the command parameters
    private record CreateProductRequest(
        string Name,
        string Description,
        decimal Price,
        int StockQuantity
    );

    private static async Task<Results<Ok<CreatedProductResponse>, BadRequest<string>>> Handle(
        CreateProductRequest request,
        IMediator mediator)
    {
        // Map request to command
        CreateProductCommand command = new(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity
        );

        // Send command to mediator
        CreatedProductResponse response = await mediator.SendAsync(command);

        // Return appropriate response
        return TypedResults.Ok(response);
    }
}
```

### 2. Create a Feature Registration Class

If this is a new feature, create a registration class to group all related endpoints:

```csharp
using NArchitecture.Starter.WebApi.Features.Inventory.Endpoints;

namespace NArchitecture.Starter.WebApi.Features.Inventory;

public static class InventoryEndpointRegistration
{
    public static RouteGroupBuilder MapInventoryEndpoints(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/inventory").WithTags("Inventory");

        // Register all inventory endpoints
        group.MapCreateProductEndpoint();
        group.MapGetAllProductsEndpoint();
        group.MapGetProductByIdEndpoint();
        // Add more endpoint mappings as needed

        return group;
    }
}
```

### 3. Register the Feature Group in the Main Registration Class

Update the `FeaturesEndpointRegistration.cs` file to include your new feature:

```csharp
using NArchitecture.Starter.WebApi.Features.Auth;
using NArchitecture.Starter.WebApi.Features.Inventory;

namespace NArchitecture.Starter.WebApi.Features;

public static class FeaturesEndpointRegistration
{
    public static WebApplication MapFeaturesEndpoints(this WebApplication app)
    {
        // Map feature endpoints
        app.MapAuthEndpoints();
        app.MapInventoryEndpoints();

        return app;
    }
}
```

## 🔄 Versioning Endpoints (Optional)

For API versioning, you can organize endpoints by version:

```csharp
// WebApi/Features/Inventory/v1/InventoryEndpointRegistrationV1.cs
public static RouteGroupBuilder MapInventoryEndpointsV1(this WebApplication app)
{
    RouteGroupBuilder group = app.MapGroup("/api/v1/inventory").WithTags("Inventory V1");

    // Register v1 endpoints

    return group;
}

// WebApi/Features/Inventory/v2/InventoryEndpointRegistrationV2.cs
public static RouteGroupBuilder MapInventoryEndpointsV2(this WebApplication app)
{
    RouteGroupBuilder group = app.MapGroup("/api/v2/inventory").WithTags("Inventory V2");

    // Register v2 endpoints

    return group;
}
```

Then in `FeaturesEndpointRegistration.cs`:

```csharp
public static WebApplication MapFeaturesEndpoints(this WebApplication app)
{
    app.MapAuthEndpoints();
    app.MapInventoryEndpointsV1();
    app.MapInventoryEndpointsV2();

    return app;
}
```

## 📊 Best Practices for API Endpoints

1. **Keep Endpoints Focused**: Each endpoint should handle a single operation.

2. **Use Meaningful Route Names**: Routes should be descriptive and follow REST conventions (`/products/{id}`).

3. **Return Appropriate Status Codes**:
   - 200 OK for successful operations
   - 201 Created for resource creation
   - 204 No Content for successful operations with no response
   - 400 Bad Request for validation errors
   - 404 Not Found when resources don't exist
   - 401 Unauthorized for authentication issues
   - 403 Forbidden for authorization issues

4. **Document Endpoints**: Always provide OpenAPI documentation using the `.WithOpenApi()` method.

5. **Use TypedResults**: Always return `TypedResults` instead of raw HTTP results for better type safety and OpenAPI integration.

6. **Consistent Error Handling**: Use the same error response format across all endpoints.

7. **Follow Resource Naming Conventions**:
   - Use plural nouns for collections (`/products`)
   - Use singular nouns with IDs for specific resources (`/products/{id}`)
   - Use verbs for non-CRUD operations (`/products/{id}/activate`)

8. **Separate Request DTOs from Commands/Queries**: Define request DTOs at the endpoint level to decouple the API contract from internal commands/queries.

By following these practices, you'll create a consistent, maintainable API that integrates seamlessly with NArchitecture's CQRS implementation.
