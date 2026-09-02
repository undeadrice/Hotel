using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Transactions.Repositories;
using Hotel.Domain.Configurations.Entities;
using Hotel.Domain.Configurations.Repositories;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Services;
using Hotel.Domain.Persistence;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Domain.Rooming.Services;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Services;

namespace Hotel.Application.Seeding;

public class SeedDataService(
    IConfigurationRepository configurationRepository,
    IUnitOfWork unitOfWork,
    INumberCycleReadRepository numberCycleReadRepository,
    INumberCycleService numberCycleService,
    ITransactionGroupCreationService transactionGroupCreationService,
    ITransactionGroupReadRepository transactionGroupReadRepository,
    ITransactionCodeCreationService transactionCodeCreationService,
    ITransactionCodeReadRepository transactionCodeReadRepository,
    IRoomTypeRepository roomTypeRepository,
    IRoomTypeReadRepository roomTypeReadRepository,
    IRoomCreationService roomCreationService,
    IRoomReadRepository roomReadRepository)
    : ISeedDataService
{
    public async Task<Guid> SeedAsync(
        string timeZoneId,
        DateOnly currentBusinessDate,
        bool seedBusinessData,
        CancellationToken cancellationToken = default)
    {
        var existing = await configurationRepository.Find(cancellationToken);

        Configuration configuration;
        await unitOfWork.StartTransaction();

        if (existing is null)
        {
            configuration = Configuration.Create(timeZoneId, currentBusinessDate);
            await configurationRepository.Add(configuration, cancellationToken);
        }
        else
        {
            configuration = existing;
        }

        configuration.MarkSeeded();
        await unitOfWork.CommitAsync();

        if (seedBusinessData)
        {
            await SeedNumberCyclesAsync(cancellationToken);
            await SeedTransactionGroupsAsync(cancellationToken);
            await SeedTransactionCodesAsync(cancellationToken);
            await SeedRoomTypesAsync(cancellationToken);
            await SeedRoomsAsync(cancellationToken);
        }

        return configuration.Id;
    }

    private async Task SeedNumberCyclesAsync(CancellationToken cancellationToken)
    {
        var existingCycles = await numberCycleReadRepository.GetAll(cancellationToken);
        if (existingCycles.Count > 0)
        {
            return;
        }

        await unitOfWork.StartTransaction();
        await numberCycleService.Create(NumberCycleTopic.Reservation, "RES", 1, cancellationToken);
        await numberCycleService.Create(NumberCycleTopic.FiscalAccount, "FA", 1, cancellationToken);
        await unitOfWork.CommitAsync();
    }

    private async Task SeedTransactionGroupsAsync(CancellationToken cancellationToken)
    {
        var existingGroups = await transactionGroupReadRepository.GetAll(null, cancellationToken);
        if (existingGroups.Count > 0)
        {
            return;
        }

        await unitOfWork.StartTransaction();
        await transactionGroupCreationService.CreateTransactionGroup("0001", "Stay", TransactionType.Charge, cancellationToken);
        await transactionGroupCreationService.CreateTransactionGroup("0002", "Food & Beverage", TransactionType.Charge, cancellationToken);
        await transactionGroupCreationService.CreateTransactionGroup("0003", "Payment", TransactionType.Payment, cancellationToken);
        await unitOfWork.CommitAsync();
    }

    private async Task SeedTransactionCodesAsync(CancellationToken cancellationToken)
    {
        var existingCodes = await transactionCodeReadRepository.GetAll(null, null, cancellationToken);
        if (existingCodes.Count > 0)
        {
            return;
        }

        var groups = await transactionGroupReadRepository.GetAll(null, cancellationToken);
        var groupByCode = groups.ToDictionary(g => g.Code, g => g.Id);

        await unitOfWork.StartTransaction();

        if (groupByCode.TryGetValue("0001", out var stayGroupId))
        {
            await transactionCodeCreationService.CreateTransactionCode(stayGroupId, "0001", "Stay", cancellationToken);
        }

        if (groupByCode.TryGetValue("0002", out var fbGroupId))
        {
            await transactionCodeCreationService.CreateTransactionCode(fbGroupId, "0002", "Breakfast", cancellationToken);
            await transactionCodeCreationService.CreateTransactionCode(fbGroupId, "0003", "Dinner", cancellationToken);
        }

        if (groupByCode.TryGetValue("0003", out var paymentGroupId))
        {
            await transactionCodeCreationService.CreateTransactionCode(paymentGroupId, "0004", "Offline payment", cancellationToken);
            await transactionCodeCreationService.CreateTransactionCode(paymentGroupId, "0005", "Online payment", cancellationToken);
        }

        await unitOfWork.CommitAsync();
    }

    private async Task SeedRoomTypesAsync(CancellationToken cancellationToken)
    {
        var existingTypes = await roomTypeReadRepository.GetAll(cancellationToken);
        if (existingTypes.Count > 0)
        {
            return;
        }

        await unitOfWork.StartTransaction();
        await roomTypeRepository.Add(RoomType.Create("Standard", "Standard room"), cancellationToken);
        await roomTypeRepository.Add(RoomType.Create("Double", "Double room"), cancellationToken);
        await roomTypeRepository.Add(RoomType.Create("King", "King room"), cancellationToken);
        await unitOfWork.CommitAsync();
    }

    private async Task SeedRoomsAsync(CancellationToken cancellationToken)
    {
        var existingRooms = await roomReadRepository.GetAll(cancellationToken);
        if (existingRooms.Count > 0)
        {
            return;
        }

        var existingTypes = await roomTypeReadRepository.GetAll(cancellationToken);
        var typeByName = existingTypes.ToDictionary(t => t.Name, t => t.Id);

        await unitOfWork.StartTransaction();

        if (typeByName.TryGetValue("Standard", out var standardId))
        {
            await roomCreationService.CreateRoom("101", standardId, cancellationToken);
            await roomCreationService.CreateRoom("102", standardId, cancellationToken);
        }

        if (typeByName.TryGetValue("Double", out var doubleId))
        {
            await roomCreationService.CreateRoom("201", doubleId, cancellationToken);
            await roomCreationService.CreateRoom("202", doubleId, cancellationToken);
        }

        if (typeByName.TryGetValue("King", out var kingId))
        {
            await roomCreationService.CreateRoom("301", kingId, cancellationToken);
            await roomCreationService.CreateRoom("302", kingId, cancellationToken);
        }

        await unitOfWork.CommitAsync();
    }
}