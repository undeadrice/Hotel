using FluentAssertions;
using Hotel.Domain.Transactions.Entities;
using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;
using Hotel.Domain.Transactions.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Transactions;

public class TransactionGroupCreationServiceTests
{
    private readonly ITransactionGroupRepository _transactionGroupRepository;
    private readonly TransactionGroupCreationService _service;

    public TransactionGroupCreationServiceTests()
    {
        _transactionGroupRepository = Substitute.For<ITransactionGroupRepository>();
        _service = new TransactionGroupCreationService(_transactionGroupRepository);
    }

    [Fact]
    public async Task CreateTransactionGroup_WhenCodeIsUnique_ShouldAddAndReturnTransactionGroup()
    {
        // Arrange
        _transactionGroupRepository.ExistsByCode("CHARGES", Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _service.CreateTransactionGroup("charges", "Charges", TransactionType.Charge);

        // Assert
        result.Code.Should().Be("CHARGES");
        result.Name.Should().Be("Charges");
        result.Type.Should().Be(TransactionType.Charge);
        result.IsActive.Should().BeTrue();

        await _transactionGroupRepository.Received(1).ExistsByCode("CHARGES", Arg.Any<CancellationToken>());
        await _transactionGroupRepository.Received(1).Add(Arg.Any<TransactionGroup>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateTransactionGroup_WhenCodeAlreadyExists_ShouldThrowTransactionGroupCodeAlreadyExistsException()
    {
        // Arrange
        _transactionGroupRepository.ExistsByCode("CHARGES", Arg.Any<CancellationToken>()).Returns(true);

        // Act
        Func<Task> act = () => _service.CreateTransactionGroup("charges", "Charges", TransactionType.Charge);

        // Assert
        await act.Should().ThrowAsync<TransactionGroupCodeAlreadyExistsException>();

        await _transactionGroupRepository.Received(1).ExistsByCode("CHARGES", Arg.Any<CancellationToken>());
        await _transactionGroupRepository.DidNotReceive().Add(Arg.Any<TransactionGroup>(), Arg.Any<CancellationToken>());
    }
}