using FluentAssertions;
using Hotel.Application.Auth.Commands;
using Hotel.Application.Auth.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Hotel.Application.Tests.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly IAuthService _authService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _authService = Substitute.For<IAuthService>();
        _handler = new LoginCommandHandler(_authService);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallAuthServiceWithCredentials()
    {
        // Arrange
        const string email = "john.doe@example.com";
        const string password = "password";
        _authService.Login(email, password).Returns("jwt-token");

        var command = new LoginCommand(email, password);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _authService.Received(1).Login(email, password);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnTokenDtoWithTokenAndValidTo()
    {
        // Arrange
        const string email = "john.doe@example.com";
        const string password = "password";
        _authService.Login(email, password).Returns("jwt-token");

        var command = new LoginCommand(email, password);
        var before = DateTime.Now.AddHours(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Token.Should().Be("jwt-token");
        result.ValidTo.Should().BeCloseTo(before, TimeSpan.FromMinutes(1));
    }
}