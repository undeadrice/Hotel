using Hotel.Domain.Folios.Entities;
using Hotel.Domain.Guests;
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

    public DbSet<ReservationGuest> ReservationGuests { get; set; }

    public DbSet<Folio> Folios { get; set; }

    public DbSet<FolioItem> FolioItems { get; set; }

    public DbSet<TransactionGroup> TransactionGroups { get; set; }

    public DbSet<TransactionCode> TransactionCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceDbContext).Assembly);
    }
}
