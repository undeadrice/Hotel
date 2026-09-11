using FluentAssertions;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.Rooming.Entities;

public class RoomTypeTests
{
    [Fact]
    public void Create_WithValidArguments_ShouldCreateActiveRoomType()
    {
        // Act
        var roomType = RoomType.Create("Deluxe", "A deluxe room");

        // Assert
        roomType.Id.Should().NotBe(Guid.Empty);
        roomType.Name.Should().Be("Deluxe");
        roomType.Description.Should().Be("A deluxe room");
        roomType.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullDescription_ShouldCreateRoomTypeWithNullDescription()
    {
        // Act
        var roomType = RoomType.Create("Standard", null);

        // Assert
        roomType.Name.Should().Be("Standard");
        roomType.Description.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowRoomTypeNameRequiredException(string? name)
    {
        // Act
        Action act = () => RoomType.Create(name!, null);

        // Assert
        act.Should().Throw<RoomTypeNameRequiredException>();
    }

    [Fact]
    public void Update_WithValidArguments_ShouldUpdateNameAndDescription()
    {
        // Arrange
        var roomType = RoomType.Create("Deluxe", "Old description");

        // Act
        roomType.Update("Suite", "Updated description");

        // Assert
        roomType.Name.Should().Be("Suite");
        roomType.Description.Should().Be("Updated description");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidName_ShouldThrowRoomTypeNameRequiredException(string? name)
    {
        // Arrange
        var roomType = RoomType.Create("Deluxe", "Description");

        // Act
        Action act = () => roomType.Update(name!, "Description");

        // Assert
        act.Should().Throw<RoomTypeNameRequiredException>();
    }
}