using Hotel.Application.Orders.Services;
using Hotel.Application.Products.Services;
using Hotel.Domain.Customers.Services;
using Hotel.Domain.Orders.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Products.Services;
using Hotel.Persistence.Customers;
using Hotel.Persistence.Orders;
using Hotel.Persistence.Products;
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

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();

        services.AddScoped(typeof(IUserOwnershipRepository<>), typeof(UserOwnershipRepository<>));

        return services;
    }
}