using Hotel.Domain.Interfaces;

namespace Hotel.Application.Pipeline;

public interface IOwnedResourceRequest<TEntity> where TEntity : IUserOwnedEntity
{
    Guid ResourceId { get; init; }
}