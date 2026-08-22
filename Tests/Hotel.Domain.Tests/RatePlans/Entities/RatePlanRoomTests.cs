using FluentAssertions;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.RatePlans.Entities;

public class RatePlanRoomTests
{
    [Fact]
    public void Create_WithValidArguments_ShouldCreateRatePlanRoom()
    {
        // Arrange
        var ratePlanId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        const decimal price = 100;

        // Act
        var room = RatePlanRoom.Create(ratePlanId, roomTypeId, price);

        // Assert
        room.RatePlanId.Should().Be(ratePlanId);
        room.RoomTypeId.Should().Be(roomTypeId);
        room.Price.Should().Be(price);
    }

    [Fact]
    public void Create_WithInvalidPrice_ShouldThrowRatePlanPriceInvalidException()
    {
        // Act
        Action act = () => RatePlanRoom.Create(Guid.NewGuid(), Guid.NewGuid(), -1);

        // Assert
        act.Should().Throw<RatePlanPriceInvalidException>();
    }
}