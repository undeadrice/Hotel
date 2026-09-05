using FluentAssertions;
using Hotel.Application.Users.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Users.CommandValidations;

public class UpdateUserCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateUserCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateUser_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.Empty, "John", "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), string.Empty, "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithEmptyLastName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "John", string.Empty, new DateOnly(1990, 1, 1), "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "John", "Doe", new DateOnly(1990, 1, 1), string.Empty, Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "John", "Doe", new DateOnly(1990, 1, 1), "not-an-email", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithFutureDateOfBirth_ReturnsBadRequest()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
        var command = new UpdateUserCommand(Guid.NewGuid(), "John", "Doe", futureDate, "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithFirstNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), new string('A', 101), "Doe", new DateOnly(1990, 1, 1), "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithLastNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "John", new string('A', 101), new DateOnly(1990, 1, 1), "john.doe@example.com", Array.Empty<Guid>());

        // Act
        var response = await _client.PutAsJsonAsync("/api/users/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}