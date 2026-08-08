using Hotel.Application.Roles.Services;
using Hotel.Application.Rooming.Services;
using Hotel.Application.Transactions.Services;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Services;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Services;

namespace Hotel.Application.Seeding;

public class SeedingService(
    IUserService userService,
    IRoleService roleService,
    ITransactionGroupCreationService transactionGroupCreationService,
    ITransactionCodeCreationService transactionCodeCreationService,
    ITransactionGroupReadRepository transactionGroupReadRepository,
    ITransactionCodeReadRepository transactionCodeReadRepository,
    IRoomTypeRepository roomTypeRepository,
    IRoomTypeReadRepository roomTypeReadRepository,
    IRoomCreationService roomCreationService,
    IRoomReadRepository roomReadRepository,
    IUnitOfWork unitOfWork)
    : ISeedingService
{
    public async Task SeedAsync()
    {
        await SeedAccountsAsync();
        await SeedTransactionGroupsAsync();
        await SeedTransactionCodesAsync();
        await SeedRoomTypesAsync();
        await SeedRoomsAsync();
    }

    private async Task SeedAccountsAsync()
    {
        var existingUsers = await userService.GetAll();
        if (existingUsers.Count > 0)
        {
            return;
        }

        string[] roles = ["Admin", "User", "Super admin"];
        var existingRoles = await roleService.GetAll();
        var roleNameToId = new Dictionary<string, Guid>();

        foreach (var roleName in roles)
        {
            var existingRole = existingRoles.FirstOrDefault(r => r.Name == roleName);
            if (existingRole != null)
            {
                roleNameToId[roleName] = existingRole.Id;
            }
            else
            {
                var roleId = await roleService.Create(roleName, Array.Empty<string>());
                roleNameToId[roleName] = roleId;
            }
        }

        var adminUser = new CreateUserContract(
            "Kamil", "Adminowski", new DateOnly(1994, 7, 18),
            "a@a.pl", "Admin123!",
            [roleNameToId["Admin"]]);

        await userService.Create(adminUser);

        var normalUser = new CreateUserContract(
            "Kamil", "Userski", new DateOnly(1994, 7, 18),
            "u@u.pl", "User123!",
            [roleNameToId["User"]]);

        await userService.Create(normalUser);

        var superAdminUser = new CreateUserContract(
            "Super", "Admin", new DateOnly(1994, 7, 18),
            "sa@sa.pl", "Admin123!",
            [roleNameToId["Super admin"]]);

        await userService.Create(superAdminUser);
    }

    private async Task SeedTransactionGroupsAsync()
    {
        await unitOfWork.StartTransaction();

        try
        {
            var existingGroups = await transactionGroupReadRepository.GetAll(null, CancellationToken.None);
            if (existingGroups.Count > 0)
            {
                await unitOfWork.RollbackAsync();
                return;
            }

            await transactionGroupCreationService.CreateTransactionGroup("0001", "Stay", TransactionType.Charge);
            await transactionGroupCreationService.CreateTransactionGroup("0002", "Food & Beverage", TransactionType.Charge);
            await transactionGroupCreationService.CreateTransactionGroup("0003", "Payment", TransactionType.Payment);

            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task SeedTransactionCodesAsync()
    {
        await unitOfWork.StartTransaction();

        try
        {
            var existingCodes = await transactionCodeReadRepository.GetAll(null, null, CancellationToken.None);
            if (existingCodes.Count > 0)
            {
                await unitOfWork.RollbackAsync();
                return;
            }

            var groups = await transactionGroupReadRepository.GetAll(null, CancellationToken.None);
            var groupByCode = groups.ToDictionary(g => g.Code, g => g.Id);

            if (groupByCode.TryGetValue("0001", out var stayGroupId))
            {
                await transactionCodeCreationService.CreateTransactionCode(stayGroupId, "0001", "Stay");
            }

            if (groupByCode.TryGetValue("0002", out var fbGroupId))
            {
                await transactionCodeCreationService.CreateTransactionCode(fbGroupId, "0002", "Breakfast");
                await transactionCodeCreationService.CreateTransactionCode(fbGroupId, "0003", "Dinner");
            }

            if (groupByCode.TryGetValue("0003", out var paymentGroupId))
            {
                await transactionCodeCreationService.CreateTransactionCode(paymentGroupId, "0004", "Offline payment");
                await transactionCodeCreationService.CreateTransactionCode(paymentGroupId, "0005", "Online payment");
            }

            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task SeedRoomTypesAsync()
    {
        await unitOfWork.StartTransaction();

        try
        {
            var existingTypes = await roomTypeReadRepository.GetAll(CancellationToken.None);
            if (existingTypes.Count > 0)
            {
                await unitOfWork.RollbackAsync();
                return;
            }

            var roomType = RoomType.Create("Standard", 150.00m, "Standard room");
            await roomTypeRepository.Add(roomType, CancellationToken.None);

            roomType = RoomType.Create("Double", 250.00m, "Double room");
            await roomTypeRepository.Add(roomType, CancellationToken.None);

            roomType = RoomType.Create("King", 400.00m, "King room");
            await roomTypeRepository.Add(roomType, CancellationToken.None);

            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task SeedRoomsAsync()
    {
        await unitOfWork.StartTransaction();

        try
        {
            var existingRooms = await roomReadRepository.GetAll(CancellationToken.None);
            if (existingRooms.Count > 0)
            {
                await unitOfWork.RollbackAsync();
                return;
            }

            var existingTypes = await roomTypeReadRepository.GetAll(CancellationToken.None);
            var typeByName = existingTypes.ToDictionary(t => t.Name, t => t.Id);

            if (typeByName.TryGetValue("Standard", out var standardId))
            {
                await roomCreationService.CreateRoom("101", standardId, CancellationToken.None);
                await roomCreationService.CreateRoom("102", standardId, CancellationToken.None);
            }

            if (typeByName.TryGetValue("Double", out var doubleId))
            {
                await roomCreationService.CreateRoom("201", doubleId, CancellationToken.None);
                await roomCreationService.CreateRoom("202", doubleId, CancellationToken.None);
            }

            if (typeByName.TryGetValue("King", out var kingId))
            {
                await roomCreationService.CreateRoom("301", kingId, CancellationToken.None);
                await roomCreationService.CreateRoom("302", kingId, CancellationToken.None);
            }

            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}