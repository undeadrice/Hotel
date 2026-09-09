using Hotel.Domain.Configurations.Entities;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.Guests;
using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Transactions.Entities;
using Hotel.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.IntegrationTests.Infrastructure;

public static class DataAccess
{
    public static IQueryable<FiscalAccount> FiscalAccount(HotelWebApplicationFactory factory)
        => Set<FiscalAccount>(factory);

    public static IQueryable<Reservation> Reservation(HotelWebApplicationFactory factory)
        => Set<Reservation>(factory);

    public static IQueryable<Guest> Guest(HotelWebApplicationFactory factory)
        => Set<Guest>(factory);

    public static IQueryable<Room> Room(HotelWebApplicationFactory factory)
        => Set<Room>(factory);

    public static IQueryable<RoomType> RoomType(HotelWebApplicationFactory factory)
        => Set<RoomType>(factory);

    public static IQueryable<RatePlan> RatePlan(HotelWebApplicationFactory factory)
        => Set<RatePlan>(factory);

    public static IQueryable<NumberCycle> NumberCycle(HotelWebApplicationFactory factory)
        => Set<NumberCycle>(factory);

    public static IQueryable<TransactionGroup> TransactionGroup(HotelWebApplicationFactory factory)
        => Set<TransactionGroup>(factory);

    public static IQueryable<TransactionCode> TransactionCode(HotelWebApplicationFactory factory)
        => Set<TransactionCode>(factory);

    public static IQueryable<Configuration> Configuration(HotelWebApplicationFactory factory)
        => Set<Configuration>(factory);

    private static IQueryable<T> Set<T>(HotelWebApplicationFactory factory)
        where T : class
    {
        var dbContext = factory.Services.CreateScope()
            .ServiceProvider
            .GetRequiredService<PersistenceDbContext>();

        return dbContext.Set<T>();
    }
}