using Hotel.Domain.Interfaces;

namespace Hotel.Domain.Interfaces;

public interface IUserOwnedEntity : IEntity
{
    Guid UserId { get; }
}