using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class UpdateTransactionGroupCommandHandlerTests
{
    private readonly ITransactionGroupUpdateService _transactionGroupUpdateService;
    private readonly UpdateTransactionGroupCommandHandler _handler;

    public UpdateTransactionGroupCommandHandlerTests()
    {
        _transactionGroupUpdateService = Substitute.For<ITransactionGroupUpdateService>();
        _handler = new UpdateTransactionGroupCommandHandler(_transactionGroupUpdateService);
    }

    [Fact]
    public async Task Handle_ShouldUpdateTransactionGroup()
    {
        // Arrange
        var id = Guid.NewGuid();
        var code = "ROOM";
        var name = "Room Charges";
        var type = TransactionType.Charge;

        var command = new UpdateTransactionGroupCommand(id, code, name, type);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _transactionGroupUpdateService.Received(1)
            .UpdateTransactionGroup(id, code, name, type, Arg.Any<CancellationToken>());
    }
}