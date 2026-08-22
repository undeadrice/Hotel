using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.Transactions;

public class TransactionCodeTests
{
    private readonly Guid _transactionGroupId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidArguments_ShouldCreateActiveTransactionCode()
    {
        // Act
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");

        // Assert
        transactionCode.Id.Should().NotBe(Guid.Empty);
        transactionCode.TransactionGroupId.Should().Be(_transactionGroupId);
        transactionCode.Code.Should().Be("ROOM");
        transactionCode.Name.Should().Be("Room charge");
        transactionCode.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldTrimAndUppercaseCodeAndTrimName()
    {
        // Act
        var transactionCode = TransactionCode.Create(_transactionGroupId, "  room  ", "  Room charge  ");

        // Assert
        transactionCode.Code.Should().Be("ROOM");
        transactionCode.Name.Should().Be("Room charge");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidCode_ShouldThrowTransactionCodeCodeRequiredException(string? code)
    {
        // Act
        Action act = () => TransactionCode.Create(_transactionGroupId, code!, "Room charge");

        // Assert
        act.Should().Throw<TransactionCodeCodeRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrowTransactionCodeNameRequiredException(string? name)
    {
        // Act
        Action act = () => TransactionCode.Create(_transactionGroupId, "ROOM", name!);

        // Assert
        act.Should().Throw<TransactionCodeNameRequiredException>();
    }

    [Fact]
    public void Update_WithValidArguments_ShouldUpdateCodeAndName()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");

        // Act
        transactionCode.Update("  payment  ", "  Payment code  ");

        // Assert
        transactionCode.Code.Should().Be("PAYMENT");
        transactionCode.Name.Should().Be("Payment code");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidCode_ShouldThrowTransactionCodeCodeRequiredException(string? code)
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");

        // Act
        Action act = () => transactionCode.Update(code!, "Room charge");

        // Assert
        act.Should().Throw<TransactionCodeCodeRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WithInvalidName_ShouldThrowTransactionCodeNameRequiredException(string? name)
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");

        // Act
        Action act = () => transactionCode.Update("ROOM", name!);

        // Assert
        act.Should().Throw<TransactionCodeNameRequiredException>();
    }

    [Fact]
    public void ChangeGroup_ShouldUpdateTransactionGroupId()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");
        var newTransactionGroupId = Guid.NewGuid();

        // Act
        transactionCode.ChangeGroup(newTransactionGroupId);

        // Assert
        transactionCode.TransactionGroupId.Should().Be(newTransactionGroupId);
    }

    [Fact]
    public void Activate_WhenInactive_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");
        transactionCode.Deactivate();

        // Act
        transactionCode.Activate();

        // Assert
        transactionCode.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(_transactionGroupId, "ROOM", "Room charge");

        // Act
        transactionCode.Deactivate();

        // Assert
        transactionCode.IsActive.Should().BeFalse();
    }
}