using Hotel.Domain.Rooming.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Domain;

public static class DIRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IRoomDeactivationService, RoomDeactivationService>();
        services.AddScoped<IRoomCreationService, RoomCreationService>();
        services.AddScoped<IRoomUpdateService, RoomUpdateService>();
        return services;
    }
}