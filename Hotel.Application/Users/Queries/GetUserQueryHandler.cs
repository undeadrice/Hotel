using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using MediatR;

namespace Hotel.Application.Users.Queries;

internal class GetUserQueryHandler(IUserService userService)
    : IRequestHandler<GetUserQuery, UserWithRolesContract>
{
    public async Task<UserWithRolesContract> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetById(request.Id);
    }
}