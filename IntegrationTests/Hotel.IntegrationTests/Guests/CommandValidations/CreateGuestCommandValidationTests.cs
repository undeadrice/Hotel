using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.CommandValidations;

public class CreateGuestCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateGuestCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateGuest_WithEmptyFirstName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand(string.Empty, "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithEmptyLastName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", string.Empty, "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithEmptyPhone_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", string.Empty, "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithEmptyEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", string.Empty, "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", "not-an-email", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithEmptyDocumentNumber_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", "john.doe@example.com", string.Empty);

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithFirstNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand(new string('A', 101), "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithLastNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", new string('A', 101), "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithPhoneExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", new string('1', 21), "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithEmailExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", new string('a', 191) + "@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateGuest_WithDocumentNumberExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", "john.doe@example.com", new string('A', 51));

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}