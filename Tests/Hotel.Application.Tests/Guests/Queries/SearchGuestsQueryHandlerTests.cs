using FluentAssertions;
using Hotel.Application.Guests.Queries;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Guests.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Guests.Queries;

public class SearchGuestsQueryHandlerTests
{
    private readonly IGuestReadRepository _guestReadRepository;
    private readonly SearchGuestsQueryHandler _handler;

    public SearchGuestsQueryHandlerTests()
    {
        _guestReadRepository = Substitute.For<IGuestReadRepository>();
        _handler = new SearchGuestsQueryHandler(_guestReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositorySearchWithFilters()
    {
        // Arrange
        const string name = "John";
        const string phone = "123456789";
        const string email = "john.doe@example.com";
        const string documentNumber = "DOC12345";

        var query = new SearchGuestsQuery(name, phone, email, documentNumber);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _guestReadRepository.Received(1).Search(
            name,
            phone,
            email,
            documentNumber,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnGuestsFromRepository()
    {
        // Arrange
        var query = new SearchGuestsQuery("John", null, null, null);

        var expected = (IReadOnlyCollection<GuestListDto>)
        [
            new GuestListDto(Guid.NewGuid(), "John Doe", "123456789", "john.doe@example.com", "DOC12345"),
        ];

        _guestReadRepository.Search("John", null, null, null, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }
}