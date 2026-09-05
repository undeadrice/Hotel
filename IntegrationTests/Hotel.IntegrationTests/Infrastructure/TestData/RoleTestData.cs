using FluentAssertions;
using Hotel.Application.Roles.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class RoleTestData
{
    public static async Task<Guid> CreateRoleAsync(
        HttpClient client,
        string? name = null,
        IReadOnlyCollection<string>? permissions = null)
    {
        var command = new CreateRoleCommand(
            name ?? $"Test Role {Guid.NewGuid():N}",
            permissions ?? new[] { "RoleView" });

        var response = await client.PostAsJsonAsync("/api/roles", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var id = await response.Content.ReadFromJsonAsync<Guid>();

        return id;
    }
}