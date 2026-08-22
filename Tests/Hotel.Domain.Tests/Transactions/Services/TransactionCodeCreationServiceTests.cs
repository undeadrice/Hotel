using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Transactions;

public class TransactionCodeCreationServiceTests
{
    private readonly ITransactionCodeRepository _transactionCodeRepository;
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly TransactionCodeCreationService _service;

    public TransactionCodeCreationServiceTests()
    {
        _transactionCodeRepository = Substitute.For<ITransactionCodeRepository>();
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _service = new TransactionCodeCreationService(_transactionCodeRepository, _transactionGroupRepository);
    }

    [Fact]
    public async Task CreateTransactionCode_WithActiveGroupAndUniqueCode_ShouldAddAndReturnTransactionCode()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroupId, Arg.Any<CancellationToken>()).Returns(transactionGroup);
        _transactionCodeRepository.ExistsByCode("ROOM", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _service.CreateTransactionCode(transactionGroupId, "room", "Room charge");

        // Assert
        result.Code.Should().Be("ROOM");
        result.Name.Should().Be("Room charge");
        result.TransactionGroupId.Should().Be(transactionGroupId);
        result.IsActive.Should().BeTrue();

        await _transactionGroupRepository.Received(1).GetById(transactionGroupId, Arg.Any<CancellationToken>());
        await _transactionCodeRepository.Received(1).ExistsByCode("ROOM", Arg.Any<CancellationToken>());
        await _transactionCodeRepository.Received(1).Add(Arg.Any<TransactionCode>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateTransactionCode_WhenGroupInactive_ShouldThrowTransactionGroupInactiveException()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);
        transactionGroup.Deactivate();

        _transactionGroupRepository.GetById(transactionGroupId, Arg.Any<CancellationToken>()).Returns(transactionGroup);

        // Act
        Func<Task> act = () => _service.CreateTransactionCode(transactionGroupId, "ROOM", "Room charge");

        // Assert
        await act.Should().ThrowAsync<TransactionGroupInactiveException>();

        await _transactionGroupRepository.Received(1).GetById(transactionGroupId, Arg.Any<CancellationToken>());
        await _transactionCodeRepository.DidNotReceive().ExistsByCode(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _transactionCodeRepository.DidNotReceive().Add(Arg.Any<TransactionCode>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateTransactionCode_WhenCodeAlreadyExists_ShouldThrowTransactionCodeAlreadyExistsException()
    {
        // Arrange
        var transactionGroupId = Guid.NewGuid();
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroupId, Arg.Any<CancellationToken>()).Returns(transactionGroup);
        _transactionCodeRepository.ExistsByCode("ROOM", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.CreateTransactionCode(transactionGroupId, "room", "Room charge");

        // Assert
        await act.Should().ThrowAsync<TransactionCodeAlreadyExistsException>();

        await _transactionCodeRepository.Received(1).ExistsByCode("ROOM", Arg.Any<CancellationToken>());
        await _transactionCodeRepository.DidNotReceive().Add(Arg.Any<TransactionCode>(), Arg.Any<CancellationToken>());
    }
}