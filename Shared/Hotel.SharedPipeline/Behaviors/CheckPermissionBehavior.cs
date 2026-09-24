using Hotel.Shared.Application.Auth;
using Hotel.Shared.Exceptions;
using Hotel.SharedPipeline.Attributes;
using MediatR;
using System.Reflection;

namespace Hotel.SharedPipeline.Behaviors;

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

        if (!currentUserService.HasPermissions(attribute.Permissions))
        {
            throw new ForbiddenException();
        }

        return await next();
    }
}
