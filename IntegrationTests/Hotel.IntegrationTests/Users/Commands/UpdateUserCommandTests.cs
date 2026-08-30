using FluentAssertions;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Contracts;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Users.Commands;

public class UpdateUserCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateUserCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateUser_WithValidCommand_ReturnsOk()
    {
        // Arrange
        var userId = await UserTestData.CreateUserAsync(_client);
        var roleId = await UserTestData.GetRoleIdAsync(_client);
        var email = $"jane.smith.{Guid.NewGuid():N}@example.com";
        var command = new UpdateUserCommand(
            userId,
            "Jane",
            "Smith",
            new DateOnly(1992, 3, 15),
            email,
            new[] { roleId });

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/api/users/{userId}");
        var user = await getResponse.Content.ReadFromJsonAsync<UserWithRolesContract>();
        user.Should().NotBeNull();
        user!.FirstName.Should().Be("Jane");
        user.LastName.Should().Be("Smith");
        user.Email.Should().Be(email);
        user.DateOfBirth.Should().Be(new DateOnly(1992, 3, 15));
    }

    [Fact]
    public async Task UpdateUser_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var roleId = await UserTestData.GetRoleIdAsync(_client);
        var command = new UpdateUserCommand(
            Guid.NewGuid(),
            "Jane",
            "Smith",
            new DateOnly(1992, 3, 15),
            $"jane.smith.{Guid.NewGuid():N}@example.com",
            new[] { roleId });

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}