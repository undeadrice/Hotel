using FluentAssertions;
using Hotel.Application.Guests.Queries;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Guests.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Guests.Queries;

public class GetGuestByIdQueryHandlerTests
{
    private readonly IGuestReadRepository _guestReadRepository;
    private readonly GetGuestByIdQueryHandler _handler;

    public GetGuestByIdQueryHandlerTests()
    {
        _guestReadRepository = Substitute.For<IGuestReadRepository>();
        _handler = new GetGuestByIdQueryHandler(_guestReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetGuestByIdQuery(id);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _guestReadRepository.Received(1).GetById(id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnGuestFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetGuestByIdQuery(id);

        var expected = new GuestDto(
            id,
            "John",
            "Doe",
            "123456789",
            "john.doe@example.com",
            "DOC12345");

        _guestReadRepository.GetById(id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }
}