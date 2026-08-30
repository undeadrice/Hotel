using FluentAssertions;
using Hotel.Application.Users.Contracts;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Users.Queries;

public class GetUsersQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetUsersQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetUsers_AfterCreatingUser_ReturnsUser()
    {
        // Arrange
        var email = $"john.doe.{Guid.NewGuid():N}@example.com";
        await UserTestData.CreateUserAsync(_client, email: email);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserContract>>();
        users.Should().ContainSingle(u => u.Email == email);
        users.Should().Contain(u => u.FirstName == "John" && u.LastName == "Doe");
    }
}