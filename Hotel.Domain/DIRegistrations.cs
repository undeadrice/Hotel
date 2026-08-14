using Hotel.Domain.NumberCycles.Services;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using Hotel.Domain.Transactions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Domain;

public static class DIRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IRoomDeactivationService, RoomDeactivationService>();
        services.AddScoped<IRoomCreationService, RoomCreationService>();
        services.AddScoped<IRoomUpdateService, RoomUpdateService>();

        services.AddScoped<IRoomAvailabilityService, RoomAvailabilityService>();

        services.AddScoped<INumberCycleService, NumberCycleService>();

        services.AddScoped<ITransactionGroupCreationService, TransactionGroupCreationService>();
        services.AddScoped<ITransactionGroupUpdateService, TransactionGroupUpdateService>();
        services.AddScoped<ITransactionCodeCreationService, TransactionCodeCreationService>();
        services.AddScoped<ITransactionCodeUpdateService, TransactionCodeUpdateService>();

        return services;
    }
}
