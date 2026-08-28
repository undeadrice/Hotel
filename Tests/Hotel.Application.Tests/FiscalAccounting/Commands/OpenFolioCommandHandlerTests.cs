using FluentAssertions;
using Hotel.Application.Common;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Commands;

public class OpenFolioCommandHandlerTests
{
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OpenFolioCommandHandler _handler;

    public OpenFolioCommandHandlerTests()
    {
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _handler = new OpenFolioCommandHandler(_fiscalAccountRepository, _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_ShouldOpenFolioAndReturnFolioId()
    {
        // Arrange
        var account = FiscalAccount.Create(Guid.NewGuid(), Guid.NewGuid(), "CY-1", DateTime.UtcNow);
        var createdAt = new DateTime(2026, 8, 11, 10, 30, 0, DateTimeKind.Utc);

        _dateTimeProvider.UtcNow.Returns(createdAt);

        _fiscalAccountRepository
            .GetById(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        var command = new OpenFolioCommand(account.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        account.Folios.Should().HaveCount(2);
        var openedFolio = account.Folios.Last();
        openedFolio.IsMainFolio.Should().BeFalse();
        openedFolio.Id.Should().Be(result);
        openedFolio.CreatedAt.Should().Be(createdAt);

        await _fiscalAccountRepository.Received(1).GetById(account.Id, Arg.Any<CancellationToken>());
    }
}