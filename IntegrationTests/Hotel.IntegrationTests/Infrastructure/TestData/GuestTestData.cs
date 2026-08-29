using FluentAssertions;
using Hotel.Application.Guests.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class GuestTestData
{
    public static async Task<Guid> CreateGuestAsync(
        HttpClient client,
        string firstName = "John",
        string lastName = "Doe",
        string phone = "123456789",
        string email = "john.doe@example.com",
        string documentNumber = "ABC123")
    {
        var response = await client.PostAsJsonAsync(
            "/api/guests",
            new CreateGuestCommand(firstName, lastName, phone, email, documentNumber));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}