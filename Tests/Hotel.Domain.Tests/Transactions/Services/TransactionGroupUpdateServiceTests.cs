using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;
using Hotel.Domain.Transactions.Repositories;

namespace Hotel.Domain.Tests.Transactions.Services;

public class TransactionGroupUpdateServiceTests
{
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly TransactionGroupUpdateService _service;

    public TransactionGroupUpdateServiceTests()
    {
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _service = new TransactionGroupUpdateService(_transactionGroupRepository);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WhenCodeUnchanged_ShouldUpdateWithoutCheckingExistence()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroup.Id, Arg.Any<CancellationToken>()).Returns(transactionGroup);

        // Act
        await _service.UpdateTransactionGroup(transactionGroup.Id, "CHARGES", "Updated charges", TransactionType.Payment);

        // Assert
        transactionGroup.Code.Should().Be("CHARGES");
        transactionGroup.Name.Should().Be("Updated charges");
        transactionGroup.Type.Should().Be(TransactionType.Payment);

        await _transactionGroupRepository.Received(1).GetById(transactionGroup.Id, Arg.Any<CancellationToken>());
        await _transactionGroupRepository.DidNotReceive().ExistsByCode(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionGroup_WhenCodeChangedAndUnique_ShouldUpdateCode()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroup.Id, Arg.Any<CancellationToken>()).Returns(transactionGroup);
        _transactionGroupRepository.ExistsByCode("PAYMENTS", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        await _service.UpdateTransactionGroup(transactionGroup.Id, "payments", "Payments", TransactionType.Payment);

        // Assert
        transactionGroup.Code.Should().Be("PAYMENTS");
        transactionGroup.Name.Should().Be("Payments");
        transactionGroup.Type.Should().Be(TransactionType.Payment);

        await _transactionGroupRepository.Received(1).ExistsByCode("PAYMENTS", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTransactionGroup_WhenCodeChangedAndAlreadyExists_ShouldThrowTransactionGroupCodeAlreadyExistsException()
    {
        // Arrange
        var transactionGroup = TransactionGroup.Create("CHARGES", "Charges", TransactionType.Charge);

        _transactionGroupRepository.GetById(transactionGroup.Id, Arg.Any<CancellationToken>()).Returns(transactionGroup);
        _transactionGroupRepository.ExistsByCode("PAYMENTS", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.UpdateTransactionGroup(transactionGroup.Id, "payments", "Payments", TransactionType.Payment);

        // Assert
        await act.Should().ThrowAsync<TransactionGroupCodeAlreadyExistsException>();
        transactionGroup.Code.Should().Be("CHARGES");

        await _transactionGroupRepository.Received(1).ExistsByCode("PAYMENTS", Arg.Any<CancellationToken>());
    }
}