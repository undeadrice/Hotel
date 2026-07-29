using Hotel.Application.Auth.Services;
using Hotel.Shared.Exceptions;
using MediatR;
using System.Reflection;

namespace Hotel.Application.Pipeline;

public class CheckPermissionBehavior<TRequest, TResponse>(ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attribute = request.GetType().GetCustomAttribute<CheckPermissionAttribute>();

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

        if (!await currentUserService.HasPermissions(attribute.Permissions))
        {
            throw new ForbiddenException();
        }

        return await next();
    }
}