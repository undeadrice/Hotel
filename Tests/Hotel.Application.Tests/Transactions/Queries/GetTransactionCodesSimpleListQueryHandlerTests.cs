using FluentAssertions;
using Hotel.Application.Transactions.Queries;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Queries;

public class GetTransactionCodesSimpleListQueryHandlerTests
{
    private readonly ITransactionCodeReadRepository _transactionCodeReadRepository;
    private readonly GetTransactionCodesSimpleListQueryHandler _handler;

    public GetTransactionCodesSimpleListQueryHandlerTests()
    {
        _transactionCodeReadRepository = Substitute.For<ITransactionCodeReadRepository>();
        _handler = new GetTransactionCodesSimpleListQueryHandler(_transactionCodeReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSimpleListFromRepository()
    {
        // Arrange
        var query = new GetTransactionCodesSimpleListQuery();

        var expected = (IReadOnlyCollection<TransactionCodeSimpleListDto>)
        [
            new TransactionCodeSimpleListDto(Guid.NewGuid(), "Room Charge"),
            new TransactionCodeSimpleListDto(Guid.NewGuid(), "Late Checkout"),
        ];

        _transactionCodeReadRepository.GetActiveSimpleList(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        await _transactionCodeReadRepository.Received(1).GetActiveSimpleList(Arg.Any<CancellationToken>());
    }
}