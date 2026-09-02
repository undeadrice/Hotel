using Hotel.Application.Roles.Services;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;

namespace Hotel.Application.Seeding;

public class SeedingService(
    IRoleService roleService,
    IUserService userService)
    : ISeedingService
{
    public async Task SeedAsync()
    {
        await SeedSuperAdminRoleAndUserAsync();
    }

    private async Task SeedSuperAdminRoleAndUserAsync()
    {
        var existingRoles = await roleService.GetAll();
        var superAdminRole = existingRoles.FirstOrDefault(r => r.Name == "Super admin");

        var superAdminRoleId = superAdminRole?.Id
            ?? await roleService.Create("Super admin", Array.Empty<string>());

        var existingUsers = await userService.GetAll();
        if (existingUsers.Any(u => u.Email == "sa@sa.pl"))
        {
            return;
        }

        var superAdminUser = new CreateUserContract(
            "Super", "Admin", new DateOnly(1994, 7, 18),
            "sa@sa.pl", "Admin123!",
            [superAdminRoleId]);

        await userService.Create(superAdminUser);
    }
}