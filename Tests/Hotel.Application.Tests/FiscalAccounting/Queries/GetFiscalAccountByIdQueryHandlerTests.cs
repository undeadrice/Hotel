using FluentAssertions;
using Hotel.Application.FiscalAccounting.Queries;
using Hotel.Application.FiscalAccounting.Repositories;
using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Domain.FiscalAccounting.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Queries;

public class GetFiscalAccountByIdQueryHandlerTests
{
    private readonly IFiscalAccountReadRepository _fiscalAccountReadRepository;
    private readonly GetFiscalAccountByIdQueryHandler _handler;

    public GetFiscalAccountByIdQueryHandlerTests()
    {
        _fiscalAccountReadRepository = Substitute.For<IFiscalAccountReadRepository>();
        _handler = new GetFiscalAccountByIdQueryHandler(_fiscalAccountReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFiscalAccountDetailsFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();

        var expected = new FiscalAccountDetailsDto(
            id,
            Guid.NewGuid(),
            "CY-1",
            "John Doe",
            DateTime.UtcNow,
            FiscalAccountStatus.Open,
            []);

        _fiscalAccountReadRepository.GetById(id, Arg.Any<CancellationToken>()).Returns(expected);

        var query = new GetFiscalAccountByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);

        await _fiscalAccountReadRepository.Received(1).GetById(id, Arg.Any<CancellationToken>());
    }
}