using Hotel.Auth.Infrastructure.Auth.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Auth.Infrastructure;

public class InfraIdentityDbContext(DbContextOptions<InfraIdentityDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{

}