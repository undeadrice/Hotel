using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.NumberCycles.Commands;

public class DeleteNumberCycleCommandHandlerTests
{
    private readonly INumberCycleService _numberCycleService;
    private readonly DeleteNumberCycleCommandHandler _handler;

    public DeleteNumberCycleCommandHandlerTests()
    {
        _numberCycleService = Substitute.For<INumberCycleService>();
        _handler = new DeleteNumberCycleCommandHandler(_numberCycleService);
    }

    [Fact]
    public async Task Handle_ShouldCallNumberCycleServiceDelete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteNumberCycleCommand(id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _numberCycleService.Received(1).Delete(id, Arg.Any<CancellationToken>());
    }
}