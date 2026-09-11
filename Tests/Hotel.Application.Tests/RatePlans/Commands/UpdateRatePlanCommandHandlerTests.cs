using FluentAssertions;
using Hotel.Application.Configurations.Services;
using Hotel.Application.RatePlans.Commands;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Commands;

public class UpdateRatePlanCommandHandlerTests
{
    private readonly IRatePlanRepository _ratePlanRepository;
    private readonly IBusinessDateProvider _businessDateProvider;
    private readonly UpdateRatePlanCommandHandler _handler;

    public UpdateRatePlanCommandHandlerTests()
    {
        _ratePlanRepository = Substitute.For<IRatePlanRepository>();
        _businessDateProvider = Substitute.For<IBusinessDateProvider>();
        _businessDateProvider
            .GetCurrentBusinessDate(Arg.Any<CancellationToken>())
            .Returns(new DateOnly(2025, 12, 31));
        _handler = new UpdateRatePlanCommandHandler(_ratePlanRepository, _businessDateProvider);
    }

    [Fact]
    public async Task Handle_ShouldLoadRatePlanAndUpdateItWithMappedCommand()
    {
        // Arrange
        var ratePlan = RatePlan.Create(
            "Peak Season",
            Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 12, 31),
            new DateOnly(2025, 12, 31),
            [new RoomTypePriceDefinition(Guid.NewGuid(), 100m)]);

        var name = "Off Season";
        var transactionCodeId = Guid.NewGuid();
        var startDate = new DateOnly(2027, 1, 1);
        var endDate = new DateOnly(2027, 6, 30);
        var roomTypeId = Guid.NewGuid();
        var rooms = new List<UpdateRatePlanRoomCommand>
        {
            new(roomTypeId, 200m),
        };

        var command = new UpdateRatePlanCommand(ratePlan.Id, name, transactionCodeId, startDate, endDate, rooms);

        _ratePlanRepository
            .GetById(ratePlan.Id, Arg.Any<CancellationToken>())
            .Returns(ratePlan);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        ratePlan.Name.Should().Be(name);
        ratePlan.TransactionCodeId.Should().Be(transactionCodeId);
        ratePlan.StartDate.Should().Be(startDate);
        ratePlan.EndDate.Should().Be(endDate);
        ratePlan.Rooms.Should().ContainSingle();
        ratePlan.Rooms.Single().RoomTypeId.Should().Be(roomTypeId);
        ratePlan.Rooms.Single().Price.Should().Be(200m);
        await _ratePlanRepository.Received(1).GetById(ratePlan.Id, Arg.Any<CancellationToken>());
    }
}