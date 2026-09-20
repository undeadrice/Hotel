using FluentValidation;
using Hotel.Application.Guests.Queries;

namespace Hotel.Application.Guests.Validators;

public class GetGuestByIdQueryValidator : AbstractValidator<GetGuestByIdQuery>
{
    public GetGuestByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Guest id is required.");
    }
}