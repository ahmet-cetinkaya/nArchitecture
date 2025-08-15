namespace NArchitecture.Starter.WebApi.Shared.Models;

/// <summary>
/// Configuration options for OpenAPI documentation.
/// </summary>
public record OpenApiConfiguration(
    string Title,
    string Version,
    string Description,
    ContactInfo Contact,
    LicenseInfo License,
    string TermsOfServiceUrl
);

/// <summary>
/// Contact information for the API documentation.
/// </summary>
public record ContactInfo(string Name, string Email);

/// <summary>
/// License information for the API.
/// </summary>
public record LicenseInfo(string Name, string Url);
