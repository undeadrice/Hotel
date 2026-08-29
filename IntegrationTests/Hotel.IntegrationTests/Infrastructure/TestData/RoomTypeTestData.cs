using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class RoomTypeTestData
{
    public static async Task<Guid> CreateRoomTypeAsync(
        HttpClient client,
        string name = "Standard",
        string? description = null)
    {
        var response = await client.PostAsJsonAsync(
            "/api/roomtypes",
            new CreateRoomTypeCommand(name, description));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}