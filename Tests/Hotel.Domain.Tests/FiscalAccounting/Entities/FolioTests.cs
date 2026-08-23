using FluentAssertions;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.FiscalAccounting.Entities;

public class FolioTests
{
    private static readonly Guid OriginatorId = Guid.NewGuid();
    private static readonly Guid OwnerId = Guid.NewGuid();
    private static readonly Guid TransactionCodeId = Guid.NewGuid();
    private static readonly DateOnly BusinessDate = DateOnly.FromDateTime(DateTime.UtcNow);

    private static Folio CreateFolio(bool isMain = true)
    {
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123");
        return isMain ? account.Folios.Single() : account.OpenFolio();
    }

    [Fact]
    public void AddItem_WithValidArguments_ShouldAddItem()
    {
        // Arrange
        var folio = CreateFolio();

        // Act
        var item = folio.AddItem("Room charge", 2, 50m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Assert
        folio.Items.Should().ContainSingle().Which.Should().Be(item);
    }

    [Fact]
    public void Settle_WhenBalanced_ShouldSetStatusSettled()
    {
        // Arrange
        var folio = CreateFolio();
        folio.AddItem("Room charge", 1, 100m, TransactionCodeId, FolioItemType.Charge, BusinessDate);
        folio.AddItem("Payment", 1, 100m, TransactionCodeId, FolioItemType.Payment, BusinessDate);

        // Act
        folio.Settle();

        // Assert
        folio.Status.Should().Be(FolioStatus.Settled);
    }

    [Fact]
    public void Settle_WhenNotBalanced_ShouldThrowFolioNotBalancedException()
    {
        // Arrange
        var folio = CreateFolio();
        folio.AddItem("Room charge", 1, 100m, TransactionCodeId, FolioItemType.Charge, BusinessDate);

        // Act
        Action act = () => folio.Settle();

        // Assert
        act.Should().Throw<FolioNotBalancedException>();
    }

    [Fact]
    public void Settle_WhenAlreadySettled_ShouldThrowFolioAlreadySettledException()
    {
        // Arrange
        var folio = CreateFolio();
        folio.Settle();

        // Act
        Action act = () => folio.Settle();

        // Assert
        act.Should().Throw<FolioAlreadySettledException>();
    }
}