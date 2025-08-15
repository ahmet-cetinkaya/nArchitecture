using NArchitecture.Core.Persistence.EntityFramework.DependencyInjection;
using NArchitecture.Starter.WebApi.Shared.Configurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add layer services
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);

// Add feature services
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use developer exception page
    _ = app.Services.UseDbMigrationApplier();

    // Use OpenAPI UI
    _ = app.UseOpenApiUI();
}

// Use feature endpoints

app.Run();
