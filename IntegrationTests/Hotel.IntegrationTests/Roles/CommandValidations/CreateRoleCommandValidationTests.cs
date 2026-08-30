using FluentAssertions;
using Hotel.Application.Roles.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.CommandValidations;

public class CreateRoleCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoleCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRole_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoleCommand(string.Empty, new[] { "RoleView" });

        // Act
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRole_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoleCommand(new string('a', 257), new[] { "RoleView" });

        // Act
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRole_WithNullPermissions_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoleCommand("Admin", null!);

        // Act
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRole_WithInvalidPermission_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoleCommand("Admin", new[] { "NotARealPermission" });

        // Act
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}