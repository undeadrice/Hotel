using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.Guests;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence;

public class PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : DbContext(options)
{
    public DbSet<Guest> Guests { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<RoomType> RoomTypes { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<FiscalAccount> FiscalAccounts { get; set; }

    public DbSet<TransactionGroup> TransactionGroups { get; set; }

    public DbSet<TransactionCode> TransactionCodes { get; set; }

    public DbSet<RatePlan> RatePlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceDbContext).Assembly);
    }
}
