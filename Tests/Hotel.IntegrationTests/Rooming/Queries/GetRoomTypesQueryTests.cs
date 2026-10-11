using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class GetRoomTypesQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRoomTypesQueryTests(HotelWebApplicationFactory factory)
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
}