using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class UpdateTransactionCodeCommandHandlerTests
{
    private readonly ITransactionCodeUpdateService _transactionCodeUpdateService;
    private readonly UpdateTransactionCodeCommandHandler _handler;

    public UpdateTransactionCodeCommandHandlerTests()
    {
        _transactionCodeUpdateService = Substitute.For<ITransactionCodeUpdateService>();
        _handler = new UpdateTransactionCodeCommandHandler(_transactionCodeUpdateService);
    }

    [Fact]
    public async Task Handle_ShouldUpdateTransactionCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        var transactionGroupId = Guid.NewGuid();
        var code = "ROOM";
        var name = "Room Charge";

        var command = new UpdateTransactionCodeCommand(id, transactionGroupId, code, name);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _transactionCodeUpdateService.Received(1)
            .UpdateTransactionCode(id, transactionGroupId, code, name, Arg.Any<CancellationToken>());
    }
}