using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

public record GetTransactionCodesQuery(Guid? TransactionGroupId = null, bool? IsActive = null)
    : IRequest<IReadOnlyCollection<TransactionCodeListDto>>;
