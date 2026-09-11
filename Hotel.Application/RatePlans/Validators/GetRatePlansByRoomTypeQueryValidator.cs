using FluentValidation;
using Hotel.Application.RatePlans.Queries;

namespace Hotel.Application.RatePlans.Validators;

public class GetRatePlansByRoomTypeQueryValidator : AbstractValidator<GetRatePlansByRoomTypeQuery>
{
    public GetRatePlansByRoomTypeQueryValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room id is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must((query, startDate) => startDate < query.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");
    }
}