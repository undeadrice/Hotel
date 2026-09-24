using Hotel.SharedPipeline.Attributes;
using Hotel.Auth.Application.Users.Contracts;
using Hotel.Shared.Application.Users.Enums;
using Hotel.Auth.Application.Users.Services;
using MediatR;

namespace Hotel.Auth.Application.Users.Queries;

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