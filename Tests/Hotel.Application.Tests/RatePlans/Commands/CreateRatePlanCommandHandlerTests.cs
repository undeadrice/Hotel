using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.RatePlans.Commands;

public class CreateRatePlanCommandHandlerTests
{
    private readonly IRatePlanRepository _ratePlanRepository;
    private readonly CreateRatePlanCommandHandler _handler;

    public CreateRatePlanCommandHandlerTests()
    {
        _ratePlanRepository = Substitute.For<IRatePlanRepository>();
        _handler = new CreateRatePlanCommandHandler(_ratePlanRepository);
    }

    [Fact]
    public async Task Handle_ShouldAddMappedRatePlanAndReturnItsId()
    {
        // Arrange
        var name = "Peak Season";
        var transactionCodeId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 12, 31);
        var roomTypeId = Guid.NewGuid();
        var rooms = new List<CreateRatePlanRoomCommand>
        {
            new(roomTypeId, 100m),
        };

        var command = new CreateRatePlanCommand(name, transactionCodeId, startDate, endDate, rooms);

        RatePlan? added = null;
        _ratePlanRepository
            .Add(Arg.Do<RatePlan>(ratePlan => added = ratePlan), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        added.Should().NotBeNull();
        result.Should().Be(added!.Id);
        added.Name.Should().Be(name);
        added.TransactionCodeId.Should().Be(transactionCodeId);
        added.StartDate.Should().Be(startDate);
        added.EndDate.Should().Be(endDate);
        added.Rooms.Should().ContainSingle();
        added.Rooms.Single().RoomTypeId.Should().Be(roomTypeId);
        added.Rooms.Single().Price.Should().Be(100m);
        await _ratePlanRepository.Received(1).Add(added, Arg.Any<CancellationToken>());
    }
}