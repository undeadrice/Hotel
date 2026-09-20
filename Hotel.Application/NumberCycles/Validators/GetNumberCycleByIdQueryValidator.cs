using FluentValidation;
using Hotel.Application.NumberCycles.Queries;

namespace Hotel.Application.NumberCycles.Validators;

public class GetNumberCycleByIdQueryValidator : AbstractValidator<GetNumberCycleByIdQuery>
{
    public GetNumberCycleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Number cycle id is required.");
    }
}