using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.Domain.Guests;
using Hotel.Domain.Guests.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Guests.Commands;

public class UpdateGuestCommandHandlerTests
{
    private readonly IGuestRepository _guestRepository;
    private readonly UpdateGuestCommandHandler _handler;

    public UpdateGuestCommandHandlerTests()
    {
        _guestRepository = Substitute.For<IGuestRepository>();
        _handler = new UpdateGuestCommandHandler(_guestRepository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldRetrieveGuestAndUpdateProfile()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateGuestCommand(
            id,
            "Jane",
            "Smith",
            "987654321",
            "jane.smith@example.com",
            "DOC54321");

        var guest = Guest.Create("John", "Doe", "123456789", "john.doe@example.com", "DOC12345");
        _guestRepository.GetById(id, Arg.Any<CancellationToken>()).Returns(guest);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _guestRepository.Received(1).GetById(id, Arg.Any<CancellationToken>());
        guest.FirstName.Should().Be("Jane");
        guest.LastName.Should().Be("Smith");
        guest.Phone.Should().Be("987654321");
        guest.Email.Should().Be("jane.smith@example.com");
        guest.DocumentNumber.Should().Be("DOC54321");
    }
}