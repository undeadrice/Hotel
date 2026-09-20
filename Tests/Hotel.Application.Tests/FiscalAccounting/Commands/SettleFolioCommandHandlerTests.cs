using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Commands;

public class SettleFolioCommandHandlerTests
{
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly SettleFolioCommandHandler _handler;

    public SettleFolioCommandHandlerTests()
    {
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _handler = new SettleFolioCommandHandler(_fiscalAccountRepository);
    }

    [Fact]
    public async Task Handle_ShouldSettleFolio()
    {
        // Arrange
        var account = FiscalAccount.Create(Guid.NewGuid(), Guid.NewGuid(), "CY-1", DateTime.UtcNow);
        var folio = account.Folios.Single();

        _fiscalAccountRepository
            .GetForSettlement(account.Id, folio.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new SettleFolioCommand(account.Id, folio.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        folio.Status.Should().Be(FolioStatus.Settled);

        await _fiscalAccountRepository.Received(1).GetForSettlement(account.Id, folio.Id, Arg.Any<CancellationToken>());
    }
}