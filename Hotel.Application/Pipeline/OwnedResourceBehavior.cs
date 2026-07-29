using Hotel.Application.Auth.Services;
using Hotel.Domain.Interfaces;
using Hotel.Shared.Exceptions;
using MediatR;

namespace Hotel.Application.Pipeline;

public class OwnedResourceBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IUserOwnershipRepository<IUserOwnedEntity> ownershipRepository)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IOwnedResourceRequest<IUserOwnedEntity>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IOwnedResourceRequest<IUserOwnedEntity>)
        {
            return await next(cancellationToken);
        }

        var ownedResourceRequest = (IOwnedResourceRequest<IUserOwnedEntity>)request;
        var userId = currentUserService.CurrentUserId ?? throw new UnauthorizedException();
        var isOwner = await ownershipRepository.IsOwner(userId, ownedResourceRequest.ResourceId);

        if (!isOwner)
        {
            throw new ForbiddenException();
        }

        return await next(cancellationToken);
    }
}