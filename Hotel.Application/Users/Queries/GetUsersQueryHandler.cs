using Hotel.Application.Pipeline;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Enums;
using Hotel.Application.Users.Services;
using MediatR;

namespace Hotel.Application.Users.Queries;

[CheckPermission(Permission.UserView)]
public record GetUsersQuery() : IRequest<IReadOnlyCollection<UserContract>>;

internal class GetUsersQueryHandler(IUserService userService)
    : IRequestHandler<GetUsersQuery, IReadOnlyCollection<UserContract>>
{
    public async Task<IReadOnlyCollection<UserContract>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetAll();
    }
}