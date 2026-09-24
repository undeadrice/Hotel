using Hotel.SharedPipeline;
using Hotel.SharedPipeline.Attributes;
using Hotel.Auth.Application.Users.Contracts;
using Hotel.Shared.Application.Users.Enums;
using Hotel.Auth.Application.Users.Services;
using MediatR;

namespace Hotel.Auth.Application.Users.Commands;

[CheckPermission(Permission.UserCreate)]
public record CreateUserCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string Password,
    IReadOnlyCollection<Guid> RoleIds) : ICommand;

internal class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var contract = new CreateUserContract(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.Password,
            request.RoleIds);

        await userService.Create(contract);
    }
}