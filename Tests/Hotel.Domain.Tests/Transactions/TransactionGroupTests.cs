using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.Transactions;

public class TransactionGroupTests
{
    [Fact]
    public void Create_WithValidArguments_ShouldCreateActiveTransactionGroup()
    {
        // Act
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);

        // Assert
        transactionGroup.Id.Should().NotBe(Guid.Empty);
        transactionGroup.Code.Should().Be("ROOM");
        transactionGroup.Name.Should().Be("Room charges");
        transactionGroup.Type.Should().Be(TransactionType.Charge);
        transactionGroup.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldTrimAndUppercaseCodeAndTrimName()
    {
        // Act
        var transactionGroup = TransactionGroup.Create("  room  ", "  Room charges  ", TransactionType.Payment);

        // Assert
        transactionGroup.Code.Should().Be("ROOM");
        transactionGroup.Name.Should().Be("Room charges");
        transactionGroup.Type.Should().Be(TransactionType.Payment);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidCode_ShouldThrowTransactionGroupCodeRequiredException(string? code)
    {
        // Act
        Action act = () => TransactionGroup.Create(code!, "Room charges", TransactionType.Charge);

        // Assert
        act.Should().Throw<TransactionGroupCodeRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowTransactionGroupNameRequiredException(string? name)
    {
        // Act
        Action act = () => TransactionGroup.Create("ROOM", name!, TransactionType.Charge);

        // Assert
        act.Should().Throw<TransactionGroupNameRequiredException>();
    }

    [Fact]
    public void Update_WithValidArguments_ShouldUpdateCodeNameAndType()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);

        // Act
        transactionGroup.Update("  payment  ", "  Payments  ", TransactionType.Payment);

        // Assert
        transactionGroup.Code.Should().Be("PAYMENT");
        transactionGroup.Name.Should().Be("Payments");
        transactionGroup.Type.Should().Be(TransactionType.Payment);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidCode_ShouldThrowTransactionGroupCodeRequiredException(string? code)
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);

        // Act
        Action act = () => transactionGroup.Update(code!, "Room charges", TransactionType.Charge);

        // Assert
        act.Should().Throw<TransactionGroupCodeRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidName_ShouldThrowTransactionGroupNameRequiredException(string? name)
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);

        // Act
        Action act = () => transactionGroup.Update("ROOM", name!, TransactionType.Charge);

        // Assert
        act.Should().Throw<TransactionGroupNameRequiredException>();
    }

    [Fact]
    public void Activate_WhenInactive_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);
        transactionGroup.Deactivate();

        // Act
        transactionGroup.Activate();

        // Assert
        transactionGroup.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room charges", TransactionType.Charge);

        // Act
        transactionGroup.Deactivate();

        // Assert
        transactionGroup.IsActive.Should().BeFalse();
    }
}