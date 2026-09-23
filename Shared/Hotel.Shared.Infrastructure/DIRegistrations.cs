using Hotel.Shared.Application.Auth;
using Hotel.Shared.Infrastructure.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Shared.Infrastructure;

public static class DIRegistrations
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
