using FluentAssertions;
using Hotel.Application.Guests.Queries;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Guests.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Guests.Queries;

public class GetGuestsQueryHandlerTests
{
    private readonly IGuestReadRepository _guestReadRepository;
    private readonly GetGuestsQueryHandler _handler;

    public GetGuestsQueryHandlerTests()
    {
        _guestReadRepository = Substitute.For<IGuestReadRepository>();
        _handler = new GetGuestsQueryHandler(_guestReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryGetAll()
    {
        // Arrange
        var query = new GetGuestsQuery();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _guestReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnGuestsFromRepository()
    {
        // Arrange
        var query = new GetGuestsQuery();

        var expected = (IReadOnlyCollection<GuestListDto>)
        [
            new GuestListDto(Guid.NewGuid(), "John Doe", "123456789", "john.doe@example.com", "DOC12345"),
            new GuestListDto(Guid.NewGuid(), "Jane Smith", "987654321", "jane.smith@example.com", "DOC54321"),
        ];

        _guestReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }
}