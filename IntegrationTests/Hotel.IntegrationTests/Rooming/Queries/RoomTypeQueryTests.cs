using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class RoomTypeQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public RoomTypeQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRoomTypes_WhenNoRoomTypesExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/roomtypes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomTypes = await response.Content.ReadFromJsonAsync<List<RoomTypeListDto>>();
        roomTypes.Should().NotBeNull();
        roomTypes.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRoomTypes_AfterCreatingRoomType_ReturnsRoomType()
    {
        // Arrange
        await RoomTypeTestData.CreateRoomTypeAsync(_client, "Deluxe", "Deluxe room");

        // Act
        var response = await _client.GetAsync("/api/roomtypes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomTypes = await response.Content.ReadFromJsonAsync<List<RoomTypeListDto>>();
        roomTypes.Should().ContainSingle(rt => rt.Name == "Deluxe");
    }

    [Fact]
    public async Task GetRoomTypeById_WithExistingRoomType_ReturnsRoomType()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Double", "Double room");

        // Act
        var response = await _client.GetAsync($"/api/roomtypes/{roomTypeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomType = await response.Content.ReadFromJsonAsync<RoomTypeDto>();
        roomType.Should().NotBeNull();
        roomType!.Id.Should().Be(roomTypeId);
        roomType.Name.Should().Be("Double");
        roomType.Description.Should().Be("Double room");
    }

    [Fact]
    public async Task GetRoomTypeById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/roomtypes/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}