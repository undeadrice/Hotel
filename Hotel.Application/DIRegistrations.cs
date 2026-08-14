using FluentValidation;
using Hotel.Application.Configurations.Services;
using Hotel.Application.Pipeline;
using Hotel.Application.Seeding;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Application;

public static class DIRegistrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DIRegistrations).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckRoleBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckPermissionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OwnedResourceBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(DIRegistrations).Assembly);

        services.AddScoped<ISeedingService, SeedingService>();

        services.AddScoped<IBusinessDateProvider, BusinessDateProvider>();

        return services;
    }
}

