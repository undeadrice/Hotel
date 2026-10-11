using FluentValidation;
using Hotel.Application.RatePlans.Queries;

namespace Hotel.Application.RatePlans.Validators;

public class GetRatePlanByIdQueryValidator : AbstractValidator<GetRatePlanByIdQuery>
{
    public GetRatePlanByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Rate plan id is required.");
    }
}