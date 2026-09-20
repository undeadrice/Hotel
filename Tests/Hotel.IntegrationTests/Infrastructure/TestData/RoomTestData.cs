using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class RoomTestData
{
    public static async Task<Guid> CreateRoomAsync(
        HttpClient client,
        string roomNumber,
        Guid roomTypeId)
    {
        var response = await client.PostAsJsonAsync(
            "/api/rooms",
            new CreateRoomCommand(roomNumber, roomTypeId));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}