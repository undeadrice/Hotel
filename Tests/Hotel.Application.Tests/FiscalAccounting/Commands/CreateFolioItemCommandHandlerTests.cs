using FluentAssertions;
using Hotel.Application.Common;
using Hotel.Application.Configurations.Services;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Commands;

public class CreateFolioItemCommandHandlerTests
{
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly IBusinessDateProvider _businessDateProvider;
    private readonly ITransactionCodeRepository _transactionCodeRepository;
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CreateFolioItemCommandHandler _handler;

    public CreateFolioItemCommandHandlerTests()
    {
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _businessDateProvider = Substitute.For<IBusinessDateProvider>();
        _transactionCodeRepository = Substitute.For<ITransactionCodeRepository>();
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _handler = new CreateFolioItemCommandHandler(
            _fiscalAccountRepository,
            _businessDateProvider,
            _transactionCodeRepository,
            _transactionGroupRepository,
            _dateTimeProvider);
    }

    [Theory]
    [InlineData(TransactionType.Charge, FolioItemType.Charge)]
    [InlineData(TransactionType.Payment, FolioItemType.Payment)]
    public async Task Handle_ShouldAddFolioItemWithMappedType(TransactionType groupType, FolioItemType expectedItemType)
    {
        // Arrange
        var group = TransactionGroup.Create("GRP", "Group", groupType);
        var code = TransactionCode.Create(group.Id, "CODE", "Code");
        var account = FiscalAccount.Create(Guid.NewGuid(), Guid.NewGuid(), "CY-1", DateTime.UtcNow);
        var folio = account.Folios.Single();
        var businessDate = new DateOnly(2026, 8, 11);
        var createdAt = new DateTime(2026, 8, 11, 10, 30, 0, DateTimeKind.Utc);

        _transactionCodeRepository.GetById(code.Id, Arg.Any<CancellationToken>()).Returns(code);
        _transactionGroupRepository.GetById(group.Id, Arg.Any<CancellationToken>()).Returns(group);
        _fiscalAccountRepository.GetByFolioId(folio.Id, Arg.Any<CancellationToken>()).Returns(account);
        _businessDateProvider.GetCurrentBusinessDate(Arg.Any<CancellationToken>()).Returns(businessDate);
        _dateTimeProvider.UtcNow.Returns(createdAt);

        var command = new CreateFolioItemCommand(folio.Id, "Room charge", 1, 100m, code.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        var item = folio.Items.Single();
        item.FolioId.Should().Be(folio.Id);
        item.Description.Should().Be("Room charge");
        item.Quantity.Should().Be(1);
        item.Amount.Should().Be(100m);
        item.TransactionCodeId.Should().Be(code.Id);
        item.TransactionType.Should().Be(expectedItemType);
        item.BusinessDate.Should().Be(businessDate);
        item.CreatedAt.Should().Be(createdAt);

        await _fiscalAccountRepository.Received(1).GetByFolioId(folio.Id, Arg.Any<CancellationToken>());
        await _transactionCodeRepository.Received(1).GetById(code.Id, Arg.Any<CancellationToken>());
        await _transactionGroupRepository.Received(1).GetById(group.Id, Arg.Any<CancellationToken>());
    }
}