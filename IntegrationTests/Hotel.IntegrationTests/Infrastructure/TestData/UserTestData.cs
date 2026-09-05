using FluentAssertions;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class UserTestData
{
    public static async Task<Guid> GetRoleIdAsync(HttpClient client)
    {
        var roles = await client.GetFromJsonAsync<List<RoleSimpleDto>>("/api/roles");

        roles.Should().NotBeNull();
        roles.Should().NotBeEmpty();

        return roles![0].Id;
    }

    public static async Task<Guid> CreateUserAsync(
        HttpClient client,
        string firstName = "John",
        string lastName = "Doe",
        DateOnly? dateOfBirth = null,
        string? email = null,
        string password = "Password123!")
    {
        email ??= $"user.{Guid.NewGuid():N}@example.com";

        var roleId = await GetRoleIdAsync(client);

        var response = await client.PostAsJsonAsync(
            "/api/users",
            new CreateUserCommand(
                firstName,
                lastName,
                dateOfBirth ?? new DateOnly(1990, 1, 1),
                email,
                password,
                new[] { roleId }));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await client.GetFromJsonAsync<List<UserContract>>("/api/users");
        var user = users!.Single(u => u.Email == email);

        return user.Id;
    }
}