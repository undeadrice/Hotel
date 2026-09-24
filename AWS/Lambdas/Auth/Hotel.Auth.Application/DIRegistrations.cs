using FluentValidation;
using Hotel.Auth.Application.Seeding;
using Hotel.SharedPipeline.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Auth.Application;

public static class DIRegistrations
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DIRegistrations).Assembly));
        services.AddValidatorsFromAssembly(typeof(DIRegistrations).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckRoleBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckPermissionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<ISeedingService, SeedingService>();

        return services;
    }
}
