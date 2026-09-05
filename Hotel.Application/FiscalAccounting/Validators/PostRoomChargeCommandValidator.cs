using FluentValidation;
using Hotel.Application.FiscalAccounting.Commands;

namespace Hotel.Application.FiscalAccounting.Validators;

public class PostRoomChargeCommandValidator : AbstractValidator<PostRoomChargeCommand>
{
    public PostRoomChargeCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation is required.");
    }
}