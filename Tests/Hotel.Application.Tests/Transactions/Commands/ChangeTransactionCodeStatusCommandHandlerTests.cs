using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class ChangeTransactionCodeStatusCommandHandlerTests
{
    private readonly ITransactionCodeRepository _transactionCodeRepository;
    private readonly ChangeTransactionCodeStatusCommandHandler _handler;

    public ChangeTransactionCodeStatusCommandHandlerTests()
    {
        _transactionCodeRepository = Substitute.For<ITransactionCodeRepository>();
        _handler = new ChangeTransactionCodeStatusCommandHandler(_transactionCodeRepository);
    }

    [Fact]
    public async Task Handle_WithIsActiveTrue_ShouldLoadTransactionCodeAndActivateIt()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(Guid.NewGuid(), "ROOM", "Room Charge");
        transactionCode.Deactivate();

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);

        var command = new ChangeTransactionCodeStatusCommand(transactionCode.Id, true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        transactionCode.IsActive.Should().BeTrue();
        await _transactionCodeRepository.Received(1).GetById(transactionCode.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithIsActiveFalse_ShouldLoadTransactionCodeAndDeactivateIt()
    {
        // Arrange
        var transactionCode = TransactionCode.Create(Guid.NewGuid(), "ROOM", "Room Charge");

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);

        var command = new ChangeTransactionCodeStatusCommand(transactionCode.Id, false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        transactionCode.IsActive.Should().BeFalse();
        await _transactionCodeRepository.Received(1).GetById(transactionCode.Id, Arg.Any<CancellationToken>());
    }
}