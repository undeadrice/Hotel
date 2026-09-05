using FluentAssertions;
using Hotel.Application.Users.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Users.CommandValidations;

public class CreateUserCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateUserCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateUser_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand(string.Empty, "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithEmptyLastName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", string.Empty, new DateOnly(1990, 1, 1), "john.doe@example.com", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", new DateOnly(1990, 1, 1), string.Empty, "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", new DateOnly(1990, 1, 1), "not-an-email", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithEmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", string.Empty, Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithTooShortPassword_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", "12345", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithFutureDateOfBirth_ReturnsBadRequest()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
        var command = new CreateUserCommand("John", "Doe", futureDate, "john.doe@example.com", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithFirstNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand(new string('A', 101), "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithLastNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand("John", new string('A', 101), new DateOnly(1990, 1, 1), "john.doe@example.com", "Password123!", Array.Empty<Guid>());

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}