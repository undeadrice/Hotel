using Hotel.Application.Pipeline;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Enums;
using Hotel.Application.Users.Services;
using MediatR;

namespace Hotel.Application.Users.Commands;

[CheckPermission(Permission.UserEdit)]
public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    IReadOnlyCollection<Guid> RoleIds) : ICommand;

internal class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var basicDataContract = new UpdateUserBasicDataContract(
            request.Id,
            request.Email,
            request.FirstName,
            request.LastName,
            request.DateOfBirth);

        await userService.UpdateBasicData(basicDataContract);
        await userService.UpdateRoles(request.Id, request.RoleIds);
    }
}