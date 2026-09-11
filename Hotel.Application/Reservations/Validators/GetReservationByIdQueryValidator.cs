using FluentValidation;
using Hotel.Application.Reservations.Queries;

namespace Hotel.Application.Reservations.Validators;

public class GetReservationByIdQueryValidator : AbstractValidator<GetReservationByIdQuery>
{
    public GetReservationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reservation id is required.");
    }
}