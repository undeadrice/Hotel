using Hotel.Domain.Folios.Entities;
using System.Linq.Expressions;

namespace Hotel.Domain.Folios.Services;

public interface IFolioRepository
{
    Task Add(Folio folio, CancellationToken token = default);

    Task Update(Folio folio, CancellationToken token = default);

    Task<Folio?> FindById(Guid id, CancellationToken token = default);

    Task<Folio> GetById(Guid id, CancellationToken token = default);

    Task<IReadOnlyCollection<Folio>> GetAll(CancellationToken token, Expression<Func<Folio, bool>>? filter = null);
}