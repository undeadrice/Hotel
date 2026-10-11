namespace Hotel.Application.RatePlans.TransferObjects;

public record RatePlanListDto(
    Guid Id,
    string Name,
    Guid TransactionCodeId,
    DateOnly StartDate,
    DateOnly EndDate,
    bool IsActive);