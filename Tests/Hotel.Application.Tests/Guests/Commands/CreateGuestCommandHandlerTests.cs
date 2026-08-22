using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.Domain.Guests;
using NSubstitute;
using Xunit;
using Hotel.Domain.Guests.Repositories;

namespace Hotel.Application.Tests.Guests.Commands;

public class CreateGuestCommandHandlerTests
{
    private readonly IGuestRepository _guestRepository;
    private readonly CreateGuestCommandHandler _handler;

    public CreateGuestCommandHandlerTests()
    {
        _guestRepository = Substitute.For<IGuestRepository>();
        _handler = new CreateGuestCommandHandler(_guestRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldAddGuestToRepositoryAndReturnGuestId()
    {
        // Arrange
        var command = new CreateGuestCommand(
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "DOC12345");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _guestRepository.Received(1).Add(Arg.Any<Guest>(), Arg.Any<CancellationToken>());
    }
}