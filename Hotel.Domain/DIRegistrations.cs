using Hotel.Domain.Orders.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Domain;

public static class DIRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IDiscountCalculator, DiscountCalculator>();
        return services;
    }
}

