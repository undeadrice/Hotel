using FluentAssertions;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Contracts;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Users.Commands;

public class CreateUserCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateUserCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateUser_WithValidCommand_ReturnsOk()
    {
        // Arrange
        var roleId = await UserTestData.GetRoleIdAsync(_client);
        var firstName = "John";
        var lastName = "Doe";
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var email = $"john.doe.{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            firstName,
            lastName,
            dateOfBirth,
            email,
            "Password123!",
            new[] { roleId });

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);
        var users = await _client.GetFromJsonAsync<List<UserContract>>("/api/users");
        var createdUser = users!.Single(u => u.Email == email);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createdUser.FirstName.Should().Be(firstName);
        createdUser.LastName.Should().Be(lastName);
        createdUser.DateOfBirth.Should().Be(dateOfBirth);
        createdUser.Email.Should().Be(email);

        var detailResponse = await _client.GetAsync($"/api/users/{createdUser.Id}");
        var detail = await detailResponse.Content.ReadFromJsonAsync<UserWithRolesContract>();
        detail.Should().NotBeNull();
        detail!.RoleIds.Should().Contain(roleId);
    }
}