using FluentAssertions;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.RatePlans.Entities;

public class RatePlanTests
{
    private static readonly DateOnly StartDate = new(2026, 1, 1);
    private static readonly DateOnly EndDate = new(2026, 12, 31);

    private static List<RoomTypePriceDefinition> ValidRooms() =>
    [
        new RoomTypePriceDefinition(Guid.NewGuid(), 100m),
        new RoomTypePriceDefinition(Guid.NewGuid(), 150m),
    ];

    [Fact]
    public void Create_WithValidArguments_ShouldCreateActiveRatePlanWithRooms()
    {
        // Arrange
        var name = "Peak Season";
        var transactionCodeId = Guid.NewGuid();
        var rooms = ValidRooms();

        // Act
        var ratePlan = RatePlan.Create(name, transactionCodeId, StartDate, EndDate, rooms);

        // Assert
        ratePlan.Id.Should().NotBe(Guid.Empty);
        ratePlan.Name.Should().Be(name);
        ratePlan.TransactionCodeId.Should().Be(transactionCodeId);
        ratePlan.StartDate.Should().Be(StartDate);
        ratePlan.EndDate.Should().Be(EndDate);
        ratePlan.IsActive.Should().BeTrue();
        ratePlan.Rooms.Should().HaveCount(2);
        ratePlan.Rooms.Select(r => r.RatePlanId).Should().AllBeEquivalentTo(ratePlan.Id);
        ratePlan.Rooms.Select(r => r.RoomTypeId).Should().Equal(rooms.Select(r => r.RoomTypeId));
        ratePlan.Rooms.Select(r => r.Price).Should().Equal(rooms.Select(r => r.Price));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowRatePlanNameRequiredException(string? name)
    {
        // Act
        Action act = () => RatePlan.Create(name!, Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanNameRequiredException>();
    }

    [Fact]
    public void Create_WithEmptyTransactionCodeId_ShouldThrowRatePlanTransactionCodeRequiredException()
    {
        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.Empty, StartDate, EndDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanTransactionCodeRequiredException>();
    }

    [Fact]
    public void Create_WhenEndDateIsEqualToStartDate_ShouldThrowRatePlanDateRangeInvalidException()
    {
        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, StartDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanDateRangeInvalidException>();
    }

    [Fact]
    public void Create_WhenEndDateIsBeforeStartDate_ShouldThrowRatePlanDateRangeInvalidException()
    {
        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.NewGuid(), EndDate, StartDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanDateRangeInvalidException>();
    }

    [Fact]
    public void Create_WithNoRooms_ShouldThrowRatePlanRoomsRequiredException()
    {
        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, []);

        // Assert
        act.Should().Throw<RatePlanRoomsRequiredException>();
    }

    [Fact]
    public void Create_WithEmptyRoomTypeId_ShouldThrowRatePlanRoomTypeRequiredException()
    {
        // Arrange
        var rooms = new List<RoomTypePriceDefinition>
        {
            new(Guid.NewGuid(), 100m),
            new(Guid.Empty, 150m),
        };

        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, rooms);

        // Assert
        act.Should().Throw<RatePlanRoomTypeRequiredException>();
    }

    [Fact]
    public void Create_WithInvalidRoomPrice_ShouldThrowRatePlanPriceInvalidException()
    {
        // Arrange
        var rooms = new List<RoomTypePriceDefinition>
        {
            new(Guid.NewGuid(), 100m),
            new(Guid.NewGuid(), -1m),
        };

        // Act
        Action act = () => RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, rooms);

        // Assert
        act.Should().Throw<RatePlanPriceInvalidException>();
    }

    [Fact]
    public void Update_WithValidArguments_ShouldUpdateRatePlanAndReplaceRooms()
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());
        var newName = "Off Season";
        var newTransactionCodeId = Guid.NewGuid();
        var newStartDate = new DateOnly(2027, 1, 1);
        var newEndDate = new DateOnly(2027, 6, 30);
        var newRooms = new List<RoomTypePriceDefinition>
        {
            new(Guid.NewGuid(), 200m),
        };

        // Act
        ratePlan.Update(newName, newTransactionCodeId, newStartDate, newEndDate, newRooms);

        // Assert
        ratePlan.Name.Should().Be(newName);
        ratePlan.TransactionCodeId.Should().Be(newTransactionCodeId);
        ratePlan.StartDate.Should().Be(newStartDate);
        ratePlan.EndDate.Should().Be(newEndDate);
        ratePlan.Rooms.Should().HaveCount(1);
        ratePlan.Rooms.Single().RoomTypeId.Should().Be(newRooms[0].RoomTypeId);
        ratePlan.Rooms.Single().Price.Should().Be(newRooms[0].Price);
        ratePlan.Rooms.Single().RatePlanId.Should().Be(ratePlan.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidName_ShouldThrowRatePlanNameRequiredException(string? name)
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Act
        Action act = () => ratePlan.Update(name!, Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanNameRequiredException>();
    }

    [Fact]
    public void Update_WithEmptyTransactionCodeId_ShouldThrowRatePlanTransactionCodeRequiredException()
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Act
        Action act = () => ratePlan.Update("Peak Season", Guid.Empty, StartDate, EndDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanTransactionCodeRequiredException>();
    }

    [Fact]
    public void Update_WhenEndDateIsEqualToStartDate_ShouldThrowRatePlanDateRangeInvalidException()
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Act
        Action act = () => ratePlan.Update("Peak Season", Guid.NewGuid(), StartDate, StartDate, ValidRooms());

        // Assert
        act.Should().Throw<RatePlanDateRangeInvalidException>();
    }

    [Fact]
    public void Update_WithNoRooms_ShouldThrowRatePlanRoomsRequiredException()
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());

        // Act
        Action act = () => ratePlan.Update("Peak Season", Guid.NewGuid(), StartDate, EndDate, []);

        // Assert
        act.Should().Throw<RatePlanRoomsRequiredException>();
    }

    [Fact]
    public void Update_WithEmptyRoomTypeId_ShouldThrowRatePlanRoomTypeRequiredException()
    {
        // Arrange
        var ratePlan = RatePlan.Create("Peak Season", Guid.NewGuid(), StartDate, EndDate, ValidRooms());
        var rooms = new List<RoomTypePriceDefinition>
        {
            new(Guid.Empty, 150m),
        };

        // Act
        Action act = () => ratePlan.Update("Peak Season", Guid.NewGuid(), StartDate, EndDate, rooms);

        // Assert
        act.Should().Throw<RatePlanRoomTypeRequiredException>();
    }
}