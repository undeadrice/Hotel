using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class CreateTransactionCodeCommandHandlerTests
{
    private readonly ITransactionCodeCreationService _transactionCodeCreationService;
    private readonly CreateTransactionCodeCommandHandler _handler;

    public CreateTransactionCodeCommandHandlerTests()
    {
        _transactionCodeCreationService = Substitute.For<ITransactionCodeCreationService>();
        _handler = new CreateTransactionCodeCommandHandler(_transactionCodeCreationService);
    }

    [Fact]
    public async Task Handle_ShouldCreateTransactionCodeAndReturnItsId()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var code = "ROOM";
        var name = "Room Charge";

        var command = new CreateTransactionCodeCommand(transactionGroupId, code, name);

        var transactionCode = TransactionCode.Create(transactionGroupId, code, name);

        _transactionCodeCreationService
            .CreateTransactionCode(transactionGroupId, code, name, Arg.Any<CancellationToken>())
            .Returns(transactionCode);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(transactionCode.Id);
        await _transactionCodeCreationService.Received(1)
            .CreateTransactionCode(transactionGroupId, code, name, Arg.Any<CancellationToken>());
    }
}