using FluentAssertions;
using Hotel.Application.Transactions.Queries;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Domain.Transactions.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Queries;

public class GetTransactionGroupsQueryHandlerTests
{
    private readonly ITransactionGroupReadRepository _transactionGroupReadRepository;
    private readonly GetTransactionGroupsQueryHandler _handler;

    public GetTransactionGroupsQueryHandlerTests()
    {
        _transactionGroupReadRepository = Substitute.For<ITransactionGroupReadRepository>();
        _handler = new GetTransactionGroupsQueryHandler(_transactionGroupReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionGroupsFromRepository()
    {
        // Arrange
        var query = new GetTransactionGroupsQuery(true);

        var expected = (IReadOnlyCollection<TransactionGroupListDto>)
        [
            new TransactionGroupListDto(Guid.NewGuid(), "ROOM", "Room Charges", TransactionType.Charge, true, 2),
            new TransactionGroupListDto(Guid.NewGuid(), "PAY", "Payments", TransactionType.Payment, true, 1),
        ];

        _transactionGroupReadRepository.GetAll(true, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _transactionGroupReadRepository.Received(1).GetAll(true, Arg.Any<CancellationToken>());
    }
}