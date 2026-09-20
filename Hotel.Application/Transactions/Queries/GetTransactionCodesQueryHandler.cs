using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionCodeView)]
public record GetTransactionCodesQuery(Guid? TransactionGroupId = null, bool? IsActive = null)
    : IRequest<IReadOnlyCollection<TransactionCodeListDto>>;

internal class GetTransactionCodesQueryHandler(ITransactionCodeReadRepository transactionCodeReadRepository)
    : IRequestHandler<GetTransactionCodesQuery, IReadOnlyCollection<TransactionCodeListDto>>
{
    public async Task<IReadOnlyCollection<TransactionCodeListDto>> Handle(
        GetTransactionCodesQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionCodeReadRepository.GetAll(
            request.TransactionGroupId,
            request.IsActive,
            cancellationToken);
    }
}
