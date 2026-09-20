using FluentAssertions;
using Hotel.Application.Transactions.Queries;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Queries;

public class GetTransactionCodeByIdQueryHandlerTests
{
    private readonly ITransactionCodeReadRepository _transactionCodeReadRepository;
    private readonly GetTransactionCodeByIdQueryHandler _handler;

    public GetTransactionCodeByIdQueryHandlerTests()
    {
        _transactionCodeReadRepository = Substitute.For<ITransactionCodeReadRepository>();
        _handler = new GetTransactionCodeByIdQueryHandler(_transactionCodeReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionCodeFromRepository()
    {
        // Arrange
        var transactionCodeId = Guid.NewGuid();
        var query = new GetTransactionCodeByIdQuery(transactionCodeId);

        var expected = new TransactionCodeDto(
            transactionCodeId,
            Guid.NewGuid(),
            "Room Charges",
            "ROOM",
            "Room Charge",
            true);

        _transactionCodeReadRepository.GetById(transactionCodeId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
        await _transactionCodeReadRepository.Received(1).GetById(transactionCodeId, Arg.Any<CancellationToken>());
    }
}