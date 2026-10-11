using FluentAssertions;
using Hotel.Application.Transactions.Queries;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Domain.Transactions.Enums;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Queries;

public class GetTransactionGroupByIdQueryHandlerTests
{
    private readonly ITransactionGroupReadRepository _transactionGroupReadRepository;
    private readonly GetTransactionGroupByIdQueryHandler _handler;

    public GetTransactionGroupByIdQueryHandlerTests()
    {
        _transactionGroupReadRepository = Substitute.For<ITransactionGroupReadRepository>();
        _handler = new GetTransactionGroupByIdQueryHandler(_transactionGroupReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionGroupFromRepository()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var query = new GetTransactionGroupByIdQuery(transactionGroupId);

        var expected = new TransactionGroupDto(
            transactionGroupId,
            "ROOM",
            "Room Charges",
            TransactionType.Charge,
            true,
            [new TransactionCodeListDto(Guid.NewGuid(), transactionGroupId, "Room Charges", "ROOM", "Room Charge", true)]);

        _transactionGroupReadRepository.GetById(transactionGroupId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
        await _transactionGroupReadRepository.Received(1).GetById(transactionGroupId, Arg.Any<CancellationToken>());
    }
}