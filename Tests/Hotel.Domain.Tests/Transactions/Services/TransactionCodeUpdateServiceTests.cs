using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Transactions.Services;

public class TransactionCodeUpdateServiceTests
{
    private readonly ITransactionCodeRepository _transactionCodeRepository;
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly TransactionCodeUpdateService _service;

    public TransactionCodeUpdateServiceTests()
    {
        _transactionCodeRepository = Substitute.For<ITransactionCodeRepository>();
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _service = new TransactionCodeUpdateService(_transactionCodeRepository, _transactionGroupRepository);
    }

    [Fact]
    public async Task UpdateTransactionCode_WhenCodeAndGroupUnchanged_ShouldUpdateWithoutCheckingExistence()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionCode = TransactionCode.Create(transactionGroupId, "ROOM", "Room charge");

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);

        // Act
        await _service.UpdateTransactionCode(transactionCode.Id, transactionGroupId, "ROOM", "Room charge");

        // Assert
        transactionCode.Code.Should().Be("ROOM");
        transactionCode.Name.Should().Be("Room charge");
        transactionCode.TransactionGroupId.Should().Be(transactionGroupId);

        await _transactionCodeRepository.Received(1).GetById(transactionCode.Id, Arg.Any<CancellationToken>());
        await _transactionCodeRepository.DidNotReceive().ExistsByCode(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _transactionGroupRepository.DidNotReceive().GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionCode_WhenCodeChangedAndUnique_ShouldUpdateCode()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionCode = TransactionCode.Create(transactionGroupId, "ROOM", "Room charge");

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);
        _transactionCodeRepository.ExistsByCode("PAYMENT", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _service.UpdateTransactionCode(transactionCode.Id, transactionGroupId, "payment", "Payment code");

        // Assert
        transactionCode.Code.Should().Be("PAYMENT");
        transactionCode.Name.Should().Be("Payment code");

        await _transactionCodeRepository.Received(1).ExistsByCode("PAYMENT", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionCode_WhenGroupChangedToActiveGroup_ShouldChangeGroupWithoutCheckingCode()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var newTransactionGroupId = Guid.NewGuid();
        var transactionCode = TransactionCode.Create(transactionGroupId, "ROOM", "Room charge");
        var newTransactionGroup = TransactionGroup.Create("PAYMENTS", "Payments", TransactionType.Payment);

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);
        _transactionGroupRepository.GetById(newTransactionGroupId, Arg.Any<CancellationToken>()).Returns(newTransactionGroup);

        // Act
        await _service.UpdateTransactionCode(transactionCode.Id, newTransactionGroupId, "ROOM", "Room charge");

        // Assert
        transactionCode.TransactionGroupId.Should().Be(newTransactionGroupId);

        await _transactionGroupRepository.Received(1).GetById(newTransactionGroupId, Arg.Any<CancellationToken>());
        await _transactionCodeRepository.DidNotReceive().ExistsByCode(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionCode_WhenGroupChangedToInactiveGroup_ShouldThrowTransactionGroupInactiveException()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var newTransactionGroupId = Guid.NewGuid();
        var transactionCode = TransactionCode.Create(transactionGroupId, "ROOM", "Room charge");
        var newTransactionGroup = TransactionGroup.Create("PAYMENTS", "Payments", TransactionType.Payment);
        newTransactionGroup.Deactivate();

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);
        _transactionGroupRepository.GetById(newTransactionGroupId, Arg.Any<CancellationToken>()).Returns(newTransactionGroup);

        // Act
        Func<Task> act = () => _service.UpdateTransactionCode(transactionCode.Id, newTransactionGroupId, "ROOM", "Room charge");

        // Assert
        await act.Should().ThrowAsync<TransactionGroupInactiveException>();
        transactionCode.TransactionGroupId.Should().Be(transactionGroupId);

        await _transactionGroupRepository.Received(1).GetById(newTransactionGroupId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionCode_WhenCodeChangedAndAlreadyExists_ShouldThrowTransactionCodeAlreadyExistsException()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionCode = TransactionCode.Create(transactionGroupId, "ROOM", "Room charge");

        _transactionCodeRepository.GetById(transactionCode.Id, Arg.Any<CancellationToken>()).Returns(transactionCode);
        _transactionCodeRepository.ExistsByCode("PAYMENT", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.UpdateTransactionCode(transactionCode.Id, transactionGroupId, "payment", "Payment code");

        // Assert
        await act.Should().ThrowAsync<TransactionCodeAlreadyExistsException>();
        transactionCode.Code.Should().Be("ROOM");

        await _transactionCodeRepository.Received(1).ExistsByCode("PAYMENT", Arg.Any<CancellationToken>());
    }
}