using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class GetRoomsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRoomsQueryTests(HotelWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.CreateDatabase();
        _client = await _factory.CreateAuthenticatedClientAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DeleteDatabase();
    }

    [Fact]
    public async Task GetRooms_WhenNoRoomsExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().NotBeNull();
        rooms.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRooms_AfterCreatingRoom_ReturnsRoom()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite", "Luxury suite");
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().ContainSingle(r => r.RoomNumber == "101");
        rooms!.Single(r => r.RoomNumber == "101").RoomType.Should().Be("Suite");
    }
}