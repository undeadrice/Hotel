using Hotel.Application.Pipeline;
using Hotel.Application.Users.Contracts;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Users.Queries;

[CheckPermission(Permission.UserView)]
public record GetUserQuery(Guid Id) : IRequest<UserWithRolesContract>;
