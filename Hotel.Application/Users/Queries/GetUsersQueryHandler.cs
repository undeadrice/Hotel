using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Services;
using MediatR;

namespace Hotel.Application.Users.Queries;

internal class GetUsersQueryHandler(IUserService userService)
    : IRequestHandler<GetUsersQuery, IReadOnlyCollection<UserContract>>
{
    public async Task<IReadOnlyCollection<UserContract>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetAll();
    }
}