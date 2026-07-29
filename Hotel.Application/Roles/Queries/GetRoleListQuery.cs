using Hotel.Application.Pipeline;
using Hotel.Application.Roles.Dtos;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Roles.Queries;

[CheckPermission(Permission.RoleView)]
public record GetRoleListQuery() : IRequest<IReadOnlyCollection<RoleSimpleDto>>;
