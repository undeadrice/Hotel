using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Transactions.Commands;

public class ChangeTransactionGroupStatusCommandHandlerTests
{
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly ChangeTransactionGroupStatusCommandHandler _handler;

    public ChangeTransactionGroupStatusCommandHandlerTests()
    {
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _handler = new ChangeTransactionGroupStatusCommandHandler(_transactionGroupRepository);
    }

    [Fact]
    public async Task Handle_WithIsActiveTrue_ShouldLoadTransactionGroupAndActivateIt()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room Charges", TransactionType.Charge);
        transactionGroup.Deactivate();

        _transactionGroupRepository.GetById(transactionGroup.Id, Arg.Any<CancellationToken>()).Returns(transactionGroup);

        var command = new ChangeTransactionGroupStatusCommand(transactionGroup.Id, true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        transactionGroup.IsActive.Should().BeTrue();
        await _transactionGroupRepository.Received(1).GetById(transactionGroup.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithIsActiveFalse_ShouldLoadTransactionGroupAndDeactivateIt()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("ROOM", "Room Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroup.Id, Arg.Any<CancellationToken>()).Returns(transactionGroup);

        var command = new ChangeTransactionGroupStatusCommand(transactionGroup.Id, false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        transactionGroup.IsActive.Should().BeFalse();
        await _transactionGroupRepository.Received(1).GetById(transactionGroup.Id, Arg.Any<CancellationToken>());
    }
}