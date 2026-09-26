using Hotel.Auth.Application.Initialization;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Auth.Infrastructure.Initialization
{
    internal class InitializationService(InfraIdentityDbContext context) : IInitializationService
    {
        public void Initialize()
        {
            Console.WriteLine("=== STARTING DATABASE MIGRATION ===");
            context.Database.Migrate();
            Console.WriteLine("=== DATABASE MIGRATED ===");
        }
    }
}
