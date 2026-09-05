using Hotel.Domain.NumberCycles.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using Hotel.Domain.Transactions.Services;
using Hotel.Persistence.Transactions;
using Hotel.Persistence.Dashboard;
using Hotel.Persistence.FiscalAccounting;
using Hotel.Persistence.Guests;
using Hotel.Persistence.Configurations;
using Hotel.Persistence.NumberCycles;
using Hotel.Persistence.RatePlans;
using Hotel.Persistence.Reservations;
using Hotel.Persistence.Rooming;
using Hotel.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hotel.Domain.RatePlans.Repositories;
using Hotel.Domain.Guests.Repositories;
using Hotel.Domain.Transactions.Repositories;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Domain.NumberCycles.Repositories;
using Hotel.Domain.Configurations.Repositories;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Application.RatePlans.Repositories;
using Hotel.Application.FiscalAccounting.Repositories;
using Hotel.Application.Dashboard.Repositories;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Reservations.Repositories;
using Hotel.Application.Rooming.Repositories;

namespace Hotel.Persistence;

public static class DIRegistrations
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersistenceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("PersistenceConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IGuestRepository, GuestRepository>();

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IRoomReadRepository, RoomReadRepository>();
        services.AddScoped<IRoomTypeReadRepository, RoomTypeReadRepository>();
        services.AddScoped<IGuestReadRepository, GuestReadRepository>();
        services.AddScoped<IDashboardReadRepository, DashboardReadRepository>();
        services.AddScoped(typeof(IUserOwnershipRepository<>), typeof(UserOwnershipRepository<>));

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationReadRepository, ReservationReadRepository>();
        services.AddScoped<IFiscalAccountRepository, FiscalAccountRepository>();
        services.AddScoped<INumberCycleRepository, NumberCycleRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

        services.AddScoped<ITransactionGroupRepository, TransactionGroupRepository>();
        services.AddScoped<ITransactionCodeRepository, TransactionCodeRepository>();
        services.AddScoped<ITransactionGroupReadRepository, TransactionGroupReadRepository>();
        services.AddScoped<ITransactionCodeReadRepository, TransactionCodeReadRepository>();
        services.AddScoped<IFiscalAccountReadRepository, FiscalAccountReadRepository>();
        services.AddScoped<INumberCycleReadRepository, NumberCycleReadRepository>();

        services.AddScoped<IRatePlanRepository, RatePlanRepository>();
        services.AddScoped<IRatePlanReadRepository, RatePlanReadRepository>();

        return services;
    }
}
