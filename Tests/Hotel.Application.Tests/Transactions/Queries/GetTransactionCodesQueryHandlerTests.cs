using FluentAssertions;
using Hotel.Application.Transactions.Queries;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Queries;

public class GetTransactionCodesQueryHandlerTests
{
    private readonly ITransactionCodeReadRepository _transactionCodeReadRepository;
    private readonly GetTransactionCodesQueryHandler _handler;

    public GetTransactionCodesQueryHandlerTests()
    {
        _transactionCodeReadRepository = Substitute.For<ITransactionCodeReadRepository>();
        _handler = new GetTransactionCodesQueryHandler(_transactionCodeReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionCodesFromRepository()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var query = new GetTransactionCodesQuery(transactionGroupId, true);

        var expected = (IReadOnlyCollection<TransactionCodeListDto>)
        [
            new TransactionCodeListDto(Guid.NewGuid(), transactionGroupId, "Room Charges", "ROOM", "Room Charge", true),
            new TransactionCodeListDto(Guid.NewGuid(), transactionGroupId, "Room Charges", "LATE", "Late Checkout", true),
        ];

        _transactionCodeReadRepository
            .GetAll(transactionGroupId, true, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _transactionCodeReadRepository.Received(1)
            .GetAll(transactionGroupId, true, Arg.Any<CancellationToken>());
    }
}