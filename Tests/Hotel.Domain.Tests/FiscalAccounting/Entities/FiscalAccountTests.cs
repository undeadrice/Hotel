using FluentAssertions;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.FiscalAccounting.Entities;

public class FiscalAccountTests
{
    private static readonly Guid OriginatorId = Guid.NewGuid();
    private static readonly Guid OwnerId = Guid.NewGuid();
    private static readonly Guid TransactionCodeId = Guid.NewGuid();
    private static readonly DateOnly BusinessDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly DateTime CreatedAt = DateTime.UtcNow;

    [Fact]
    public void Create_WithValidArguments_ShouldSetPropertiesAndCreateMainFolio()
    {
        // Act
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);

        // Assert
        account.Id.Should().NotBe(Guid.Empty);
        account.OriginatorId.Should().Be(OriginatorId);
        account.OwnerId.Should().Be(OwnerId);
        account.CycleIdentifier.Should().Be("CY-123");
        account.Status.Should().Be(FiscalAccountStatus.Open);
        account.CreatedAt.Should().Be(CreatedAt);

        account.Folios.Should().HaveCount(1);
        account.Folios.Single().IsMainFolio.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenCycleIdentifierInvalid_ShouldThrowFiscalAccountCycleIdentifierRequiredException(string? cycleIdentifier)
    {
        // Act
        Action act = () => FiscalAccount.Create(OriginatorId, OwnerId, cycleIdentifier!, CreatedAt);

        // Assert
        act.Should().Throw<FiscalAccountCycleIdentifierRequiredException>();
    }

    [Fact]
    public void OpenFolio_ShouldAddNewFolioThatIsNotMain()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);

        // Act
        var folio = account.OpenFolio(CreatedAt);

        // Assert
        folio.Should().NotBeNull();
        folio.IsMainFolio.Should().BeFalse();
        account.Folios.Should().HaveCount(2);
    }

    [Fact]
    public void OpenFolio_WhenAccountCheckedOut_ShouldThrowFiscalAccountAlreadyCheckedOutException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        account.SettleFolio(account.Folios.Single().Id);
        account.CheckOut();

        // Act
        Action act = () => account.OpenFolio(CreatedAt);

        // Assert
        act.Should().Throw<FiscalAccountAlreadyCheckedOutException>();
    }

    [Fact]
    public void AddFolioItem_ShouldAddItemToFolio()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        var folio = account.Folios.Single();

        // Act
        var item = account.AddFolioItem(
            folio.Id,
            "Room charge",
            1,
            100m,
            TransactionCodeId,
            FolioItemType.Charge,
            BusinessDate,
            CreatedAt);

        // Assert
        item.Should().NotBeNull();
        item.FolioId.Should().Be(folio.Id);
        item.Description.Should().Be("Room charge");
        item.Amount.Should().Be(100m);
        folio.Items.Should().HaveCount(1);
    }

    [Fact]
    public void AddFolioItem_WhenFolioNotFound_ShouldThrowFolioNotFoundException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);

        // Act
        Action act = () => account.AddFolioItem(
            Guid.NewGuid(),
            "Room charge",
            1,
            100m,
            TransactionCodeId,
            FolioItemType.Charge,
            BusinessDate,
            CreatedAt);

        // Assert
        act.Should().Throw<FolioNotFoundException>();
    }

    [Fact]
    public void AddFolioItem_WhenAccountCheckedOut_ShouldThrowFiscalAccountAlreadyCheckedOutException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        var folio = account.Folios.Single();
        account.SettleFolio(folio.Id);
        account.CheckOut();

        // Act
        Action act = () => account.AddFolioItem(
            folio.Id,
            "Room charge",
            1,
            100m,
            TransactionCodeId,
            FolioItemType.Charge,
            BusinessDate,
            CreatedAt);

        // Assert
        act.Should().Throw<FiscalAccountAlreadyCheckedOutException>();
    }

    [Fact]
    public void SettleFolio_WhenFolioNotFound_ShouldThrowFolioNotFoundException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);

        // Act
        Action act = () => account.SettleFolio(Guid.NewGuid());

        // Assert
        act.Should().Throw<FolioNotFoundException>();
    }

    [Fact]
    public void PostChargeToMainFolio_ShouldAddChargeItemToMainFolio()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        var mainFolio = account.Folios.Single();

        // Act
        var item = account.PostChargeToMainFolio("Room charge", 150m, TransactionCodeId, BusinessDate, CreatedAt);

        // Assert
        item.Should().NotBeNull();
        item.FolioId.Should().Be(mainFolio.Id);
        item.TransactionType.Should().Be(FolioItemType.Charge);
        item.Quantity.Should().Be(1);
        item.Amount.Should().Be(150m);
        item.TransactionCodeId.Should().Be(TransactionCodeId);
        item.BusinessDate.Should().Be(BusinessDate);
        mainFolio.Items.Should().HaveCount(1);
    }

    [Fact]
    public void CheckOut_WhenAllFoliosSettled_ShouldSetStatusCheckedOut()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        account.SettleFolio(account.Folios.Single().Id);

        // Act
        account.CheckOut();

        // Assert
        account.Status.Should().Be(FiscalAccountStatus.CheckedOut);
    }

    [Fact]
    public void CheckOut_WhenAlreadyCheckedOut_ShouldThrowFiscalAccountAlreadyCheckedOutException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);
        account.SettleFolio(account.Folios.Single().Id);
        account.CheckOut();

        // Act
        Action act = () => account.CheckOut();

        // Assert
        act.Should().Throw<FiscalAccountAlreadyCheckedOutException>();
    }

    [Fact]
    public void CheckOut_WhenFolioNotSettled_ShouldThrowFiscalAccountNotSettledException()
    {
        // Arrange
        var account = FiscalAccount.Create(OriginatorId, OwnerId, "CY-123", CreatedAt);

        // Act
        Action act = () => account.CheckOut();

        // Assert
        act.Should().Throw<FiscalAccountNotSettledException>();
    }
}