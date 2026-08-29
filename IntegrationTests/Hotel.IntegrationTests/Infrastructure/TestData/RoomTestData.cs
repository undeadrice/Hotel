using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.TransferObjects;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class RoomTestData
{
    public static async Task CreateRoomAsync(
        HttpClient client,
        string roomNumber,
        Guid roomTypeId)
    {
        var response = await client.PostAsJsonAsync(
            "/api/rooms",
            new CreateRoomCommand(roomNumber, roomTypeId));

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public static async Task<Guid> GetRoomIdAsync(
        HttpClient client,
        string roomNumber)
    {
        var response = await client.GetAsync("/api/rooms");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().NotBeNull();

        return rooms!.Single(r => r.RoomNumber == roomNumber).Id;
    }
}