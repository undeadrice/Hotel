using FluentAssertions;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.FiscalAccounting.Entities;

public class FolioItemTests
{
    private static readonly Guid FolioId = Guid.NewGuid();
    private static readonly Guid TransactionCodeId = Guid.NewGuid();
    private static readonly DateOnly BusinessDate = DateOnly.FromDateTime(DateTime.UtcNow);

    [Fact]
    public void Create_WithValidArguments_ShouldSetProperties()
    {
        // Act
        var item = FolioItem.Create(FolioId, "Room charge", 2, 50m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Assert
        item.Id.Should().NotBe(Guid.Empty);
        item.FolioId.Should().Be(FolioId);
        item.Description.Should().Be("Room charge");
        item.Quantity.Should().Be(2);
        item.Amount.Should().Be(50m);
        item.TransactionCodeId.Should().Be(TransactionCodeId);
        item.TransactionType.Should().Be(FolioItemType.Charge);
        item.BusinessDate.Should().Be(BusinessDate);
        item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenDescriptionInvalid_ShouldThrowInvalidFolioItemDescriptionException(string? description)
    {
        // Act
        Action act = () => FolioItem.Create(FolioId, description!, 1, 50m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Assert
        act.Should().Throw<InvalidFolioItemDescriptionException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WhenQuantityInvalid_ShouldThrowInvalidFolioItemQuantityException(int quantity)
    {
        // Act
        Action act = () => FolioItem.Create(FolioId, "Room charge", quantity, 50m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Assert
        act.Should().Throw<InvalidFolioItemQuantityException>();
    }

    [Fact]
    public void Create_WhenAmountNegative_ShouldThrowInvalidFolioItemAmountException()
    {
        // Act
        Action act = () => FolioItem.Create(FolioId, "Room charge", 1, -1m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Assert
        act.Should().Throw<InvalidFolioItemAmountException>();
    }
}