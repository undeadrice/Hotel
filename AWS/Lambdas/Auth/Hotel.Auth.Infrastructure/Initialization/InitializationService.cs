using Hotel.Auth.Application.Initialization;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Auth.Infrastructure.Initialization
{
    internal class InitializationService(InfraIdentityDbContext context) : IInitializationService
    {
        public void Initialize()
        {
            context.Database.Migrate();
        }
    }
}
