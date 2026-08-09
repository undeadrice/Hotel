using FluentValidation;
using Hotel.Application.Reservations.Commands;

namespace Hotel.Application.Reservations.Validators;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.CreatorId)
            .NotEmpty().WithMessage("Creator guest is required.");

        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room is required.");

        RuleFor(x => x.RatePlanId)
            .NotEmpty().WithMessage("RatePlan is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must((command, startDate) => startDate < command.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");

        RuleFor(x => x.GuestIds)
            .NotNull().WithMessage("Guests list is required.")
            .Must(g => g.Count > 0).WithMessage("At least one guest is required.");
    }
}