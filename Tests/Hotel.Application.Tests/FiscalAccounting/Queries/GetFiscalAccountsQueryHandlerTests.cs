using FluentAssertions;
using Hotel.Application.FiscalAccounting.Queries;
using Hotel.Application.FiscalAccounting.Repositories;
using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Domain.FiscalAccounting.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Queries;

public class GetFiscalAccountsQueryHandlerTests
{
    private readonly IFiscalAccountReadRepository _fiscalAccountReadRepository;
    private readonly GetFiscalAccountsQueryHandler _handler;

    public GetFiscalAccountsQueryHandlerTests()
    {
        _fiscalAccountReadRepository = Substitute.For<IFiscalAccountReadRepository>();
        _handler = new GetFiscalAccountsQueryHandler(_fiscalAccountReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFiscalAccountsFromRepository()
    {
        // Arrange
        var query = new GetFiscalAccountsQuery();

        var expected = (IReadOnlyCollection<FiscalAccountListItemDto>)
        [
            new FiscalAccountListItemDto(Guid.NewGuid(), "CY-1", DateTime.UtcNow, "John Doe", FiscalAccountStatus.Open),
            new FiscalAccountListItemDto(Guid.NewGuid(), "CY-2", DateTime.UtcNow, "Jane Doe", FiscalAccountStatus.CheckedOut),
        ];

        _fiscalAccountReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        await _fiscalAccountReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}