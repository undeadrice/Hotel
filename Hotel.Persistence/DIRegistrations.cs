using Hotel.Application.Guests.Services;
using Hotel.Application.Rooming.Services;
using Hotel.Domain.Folios.Services;
using Hotel.Domain.Guests.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using Hotel.Domain.Transactions.Services;
using Hotel.Application.Dashboard.Services;
using Hotel.Application.Transactions.Services;
using Hotel.Persistence.Transactions;
using Hotel.Persistence.Dashboard;
using Hotel.Persistence.Folios;
using Hotel.Persistence.Guests;
using Hotel.Persistence.Reservations;
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

        services.AddScoped<IGuestRepository, GuestRepository>();

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IRoomReadRepository, RoomReadRepository>();
        services.AddScoped<IRoomTypeReadRepository, RoomTypeReadRepository>();
        services.AddScoped<IGuestReadRepository, GuestReadRepository>();
        services.AddScoped<IDashboardReadRepository, DashboardReadRepository>();
        services.AddScoped(typeof(IUserOwnershipRepository<>), typeof(UserOwnershipRepository<>));

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IFolioRepository, FolioRepository>();

        services.AddScoped<ITransactionGroupRepository, TransactionGroupRepository>();
        services.AddScoped<ITransactionCodeRepository, TransactionCodeRepository>();
        services.AddScoped<ITransactionGroupReadRepository, TransactionGroupReadRepository>();
        services.AddScoped<ITransactionCodeReadRepository, TransactionCodeReadRepository>();

        return services;
    }
}
