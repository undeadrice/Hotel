using Hotel.Domain.Guests;
using Hotel.Domain.Rooming.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence;

public class PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : DbContext(options)
{
    public DbSet<Guest> Guests { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceDbContext).Assembly);
    }
}
