using Hotel.Application.Customers.Services;
using Hotel.Application.Rooming.Services;
using Hotel.Domain.Customers.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Rooming.Services;
using Hotel.Persistence.Customers;
using Hotel.Persistence.Rooming;
using Hotel.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Persistence;

public static class DIRegistrations
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersistenceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PersistenceConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IRoomReadRepository, RoomReadRepository>();
        services.AddScoped<IRoomTypeReadRepository, RoomTypeReadRepository>();
        services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        services.AddScoped(typeof(IUserOwnershipRepository<>), typeof(UserOwnershipRepository<>));

        return services;
    }
}