using FluentValidation;
using Hotel.Application.RatePlans.Commands;

namespace Hotel.Application.RatePlans.Validators;

public class CreateRatePlanCommandValidator : AbstractValidator<CreateRatePlanCommand>
{
    public CreateRatePlanCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.TransactionCodeId)
            .NotEmpty().WithMessage("Transaction code is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must((command, startDate) => startDate < command.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");

        RuleFor(x => x.Rooms)
            .NotNull().WithMessage("Rooms list is required.")
            .Must(rooms => rooms.Count > 0).WithMessage("At least one room is required.");

        RuleForEach(x => x.Rooms).SetValidator(new CreateRatePlanRoomCommandValidator());
    }
}