using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using MediatR;

namespace Hotel.Application.Users.Commands;

public class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand>
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