using Hotel.Application.Auth.Services;
using Hotel.Shared.Exceptions;
using MediatR;
using System.Reflection;

namespace Hotel.Application.Pipeline;

public class CheckRoleBehavior<TRequest, TResponse>(ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attribute = request.GetType().GetCustomAttribute<CheckRoleAttribute>();

        if (attribute is null)
        {
            return await next();
        }

        if (!currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedException();
        }

        if (await currentUserService.IsSuperAdmin())
        {
            return await next();
        }

        foreach (var role in attribute.Roles)
        {
            if (!await currentUserService.IsInRole(role))
            {
                throw new ForbiddenException($"User does not have required role: {role}");
            }
        }

        return await next();
    }
}