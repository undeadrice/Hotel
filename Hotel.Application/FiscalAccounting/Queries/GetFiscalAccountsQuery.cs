using Hotel.Application.FiscalAccounting.TransferObjects;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Queries;

public record GetFiscalAccountsQuery : IRequest<IReadOnlyCollection<FiscalAccountListItemDto>>;