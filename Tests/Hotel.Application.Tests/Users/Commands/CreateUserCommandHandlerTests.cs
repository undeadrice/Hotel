using FluentAssertions;
using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new CreateUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallUserServiceWithMappedContract()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        var dateOfBirth = new DateOnly(1990, 1, 1);
        const string email = "john.doe@example.com";
        const string password = "password";
        var roleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var command = new CreateUserCommand(firstName, lastName, dateOfBirth, email, password, roleIds);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userService.Received(1).Create(Arg.Is<CreateUserContract>(contract =>
            contract!.FirstName == firstName &&
            contract.LastName == lastName &&
            contract.DateOfBirth == dateOfBirth &&
            contract.Email == email &&
            contract.Password == password &&
            contract.RoleIds == roleIds));
    }
}