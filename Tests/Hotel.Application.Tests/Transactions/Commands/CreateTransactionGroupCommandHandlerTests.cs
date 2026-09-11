using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class CreateTransactionGroupCommandHandlerTests
{
    private readonly ITransactionGroupCreationService _transactionGroupCreationService;
    private readonly CreateTransactionGroupCommandHandler _handler;

    public CreateTransactionGroupCommandHandlerTests()
    {
        _transactionGroupCreationService = Substitute.For<ITransactionGroupCreationService>();
        _handler = new CreateTransactionGroupCommandHandler(_transactionGroupCreationService);
    }

    [Fact]
    public async Task Handle_ShouldCreateTransactionGroupAndReturnItsId()
    {
        // Arrange
        var code = "ROOM";
        var name = "Room Charges";
        var type = TransactionType.Charge;

        var command = new CreateTransactionGroupCommand(code, name, type);

        var transactionGroup = TransactionGroup.Create(code, name, type);

        _transactionGroupCreationService
            .CreateTransactionGroup(code, name, type, Arg.Any<CancellationToken>())
            .Returns(transactionGroup);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(transactionGroup.Id);
        await _transactionGroupCreationService.Received(1)
            .CreateTransactionGroup(code, name, type, Arg.Any<CancellationToken>());
    }
}