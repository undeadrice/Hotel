using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.CommandValidations;

public class UpdateGuestCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateGuestCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateGuest_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.Empty, "John", "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), string.Empty, "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmptyLastName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", string.Empty, "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmptyPhone_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", string.Empty, "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", string.Empty, "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", "not-an-email", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmptyDocumentNumber_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", "john.doe@example.com", string.Empty);

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithFirstNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), new string('A', 101), "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithLastNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", new string('A', 101), "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithPhoneExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", new string('1', 21), "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithEmailExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", new string('a', 191) + "@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGuest_WithDocumentNumberExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", "john.doe@example.com", new string('A', 51));

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}