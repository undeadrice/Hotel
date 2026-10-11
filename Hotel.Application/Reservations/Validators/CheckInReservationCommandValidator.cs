using FluentValidation;
using Hotel.Application.Reservations.Commands;

namespace Hotel.Application.Reservations.Validators;

public class CheckInReservationCommandValidator : AbstractValidator<CheckInReservationCommand>
{
    public CheckInReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation id is required.");
    }
}