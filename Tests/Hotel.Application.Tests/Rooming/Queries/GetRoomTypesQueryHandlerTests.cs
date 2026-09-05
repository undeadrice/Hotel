using FluentAssertions;
using Hotel.Application.Rooming.Queries;
using Hotel.Application.Rooming.Repositories;
using Hotel.Application.Rooming.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Rooming.Queries;

public class GetRoomTypesQueryHandlerTests
{
    private readonly IRoomTypeReadRepository _roomTypeReadRepository;
    private readonly GetRoomTypesQueryHandler _handler;

    public GetRoomTypesQueryHandlerTests()
    {
        _roomTypeReadRepository = Substitute.For<IRoomTypeReadRepository>();
        _handler = new GetRoomTypesQueryHandler(_roomTypeReadRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnRoomTypesFromRepository()
    {
        // Arrange
        var query = new GetRoomTypesQuery();

        var expected = (IReadOnlyCollection<RoomTypeListDto>)
        [
            new RoomTypeListDto(Guid.NewGuid(), "Standard"),
            new RoomTypeListDto(Guid.NewGuid(), "Deluxe"),
        ];

        _roomTypeReadRepository.GetAll(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expected);
        await _roomTypeReadRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}