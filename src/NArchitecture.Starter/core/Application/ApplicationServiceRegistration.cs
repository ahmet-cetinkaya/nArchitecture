using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Application.DependencyInjection;
using NArchitecture.Core.Localization.Resource.Yaml.DependencyInjection;
using NArchitecture.Core.Mapper.Mapster.DependencyInjection;
using NArchitecture.Core.Mediator;
using NArchitecture.Core.Validation.FluentValidation.DependencyInjection;

namespace NArchitecture.Starter.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Get the current assembly that contains our handlers
        Assembly currentAssembly = typeof(ApplicationServiceRegistration).Assembly;

        // Register NArchitecture.Core.Mediator.Abstractions.IMediator service with explicit assembly scanning
        _ = services.AddMediator(currentAssembly);

        // Register NArchitecture.Core.Application.IBusinessRules services
        _ = services.AddBusinessRules();

        // Register NArchitecture.Core.Validation.Abstractions.IValidator<T> services for all FluentValidation.IValidator<T> services
        _ = services.AddFluentValidation(assemblies: [currentAssembly]);

        // Register NArchitecture.Core.Mapping.Abstractions.IMapper services for all Mapster.IRegister services
        _ = services.AddNArchitectureMapster(currentAssembly);

        // Register NArchitecture.Core.Localization.Abstractions.ILocalizationService service with YamlResourceLocalization
        _ = services.AddYamlResourceLocalization();

        return services;
    }
}
