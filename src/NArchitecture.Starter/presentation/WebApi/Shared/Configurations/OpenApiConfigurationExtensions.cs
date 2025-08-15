using NArchitecture.Starter.WebApi.Shared.Models;
using Scalar.AspNetCore;

namespace NArchitecture.Starter.WebApi.Shared.Configurations;

/// <summary>
/// Extension methods for configuring OpenAPI in the application.
/// </summary>
public static class OpenApiConfigurationExtensions
{
    /// <summary>
    /// Adds OpenAPI services to the service collection based on provided configuration.
    /// </summary>
    /// <param name="services">The service collection to add OpenAPI services to.</param>
    /// <param name="configuration">The application configuration.</param>
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        OpenApiConfiguration openApiOptions = configuration.GetOpenApiConfiguration();

        _ = services.AddOpenApi(options =>
        {
            _ = options.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = openApiOptions.Title,
                        Version = openApiOptions.Version,
                        Description = openApiOptions.Description,
                        Contact = new() { Name = openApiOptions.Contact.Name, Email = openApiOptions.Contact.Email },
                        License = new() { Name = openApiOptions.License.Name, Url = new Uri(openApiOptions.License.Url) },
                        TermsOfService = new Uri(openApiOptions.TermsOfServiceUrl),
                    };
                    return Task.CompletedTask;
                }
            );
        });

        return services;
    }

    /// <summary>
    /// Gets the OpenAPI configuration from the application configuration.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public static OpenApiConfiguration GetOpenApiConfiguration(this IConfiguration configuration)
    {
        OpenApiConfiguration openApiOptions =
            configuration.GetSection("OpenApi").Get<OpenApiConfiguration>()
            ?? throw new InvalidOperationException("OpenApi configuration is not found.");
        return openApiOptions;
    }

    /// <summary>
    /// Configures the application to use Scalar API reference.
    /// </summary>
    /// <param name="app">The application to configure.</param>
    public static IApplicationBuilder UseOpenApiUI(this IApplicationBuilder app)
    {
        // Map OpenAPI endpoints
        _ = app.UseRouting();
        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapOpenApi();

            // Map Scalar API reference
            _ = endpoints.MapScalarApiReference(options =>
            {
                options.Theme = ScalarTheme.Kepler;
                options.CustomCss = """
                .light-mode {
                    --scalar-color-1: #0a2a42;
                    --scalar-color-2: #336699;
                    --scalar-color-3: #4a90e2;
                    --scalar-color-accent: #7fa6d9;
                    --scalar-background-1: #f5faff;
                }
                .dark-mode {
                    --scalar-color-1: #e0f2ff;
                    --scalar-color-2: #a1c4fd;
                    --scalar-color-3: #7fa6d9;
                    --scalar-color-accent: #4a90e2;
                    --scalar-background-1:rgb(0, 11, 18);
                }
                """;
                options.DarkMode = true; // Default theme mode
                options.HideClientButton = true;
            });
        });

        return app;
    }
}
