using Hotel.Application.Users.Commands;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly IUserService _userService;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new UpdateUserCommandHandler(_userService);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallUpdateBasicDataWithMappedContract()
    {
        // Arrange
        var id = Guid.NewGuid();
        const string firstName = "John";
        const string lastName = "Doe";
        var dateOfBirth = new DateOnly(1990, 1, 1);
        const string email = "john.doe@example.com";
        var roleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var command = new UpdateUserCommand(id, firstName, lastName, dateOfBirth, email, roleIds);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userService.Received(1).UpdateBasicData(Arg.Is<UpdateUserBasicDataContract>(contract =>
            contract!.Id == id &&
            contract.Email == email &&
            contract.FirstName == firstName &&
            contract.LastName == lastName &&
            contract.DateOfBirth == dateOfBirth));
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallUpdateRolesWithIdAndRoleIds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var roleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var command = new UpdateUserCommand(
            id,
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "john.doe@example.com",
            roleIds);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userService.Received(1).UpdateRoles(id, roleIds);
    }
}