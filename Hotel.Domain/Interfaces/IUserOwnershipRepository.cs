namespace Hotel.Domain.Interfaces
{
    public interface IUserOwnershipRepository<T> where T : IUserOwnedEntity
    {
        Task<bool> IsOwner(Guid id, Guid resourceId);
    }
}