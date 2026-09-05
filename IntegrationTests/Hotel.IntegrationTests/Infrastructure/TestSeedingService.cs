using Hotel.Application.Roles.Services;
using Hotel.Application.Seeding;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Enums;
using Hotel.Application.Users.Services;
using Hotel.Infrastructure;

namespace Hotel.IntegrationTests.Infrastructure;

public class TestSeedingService(
    InfraIdentityDbContext dbContext,
    IUserService userService,
    IRoleService roleService) : ISeedingService
{
    public async Task SeedAsync()
    {
        await dbContext.Database.EnsureCreatedAsync();

        var existingUsers = await userService.GetAll();
        if (existingUsers.Count > 0)
        {
            return;
        }

        var roleName = UserRole.SuperAdmin.ToString();

        var existingRoles = await roleService.GetAll();
        var existingRole = existingRoles.FirstOrDefault(r => r.Name == roleName);

        var roleId = existingRole?.Id ?? await roleService.Create(roleName, Array.Empty<string>());

        var superAdminUser = new CreateUserContract(
            "Super", "Admin", new DateOnly(1994, 7, 18),
            "sa@sa.pl", "Admin123!",
            [roleId]);

        await userService.Create(superAdminUser);
    }
}