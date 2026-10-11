using FluentValidation;
using Hotel.Application.RatePlans.Commands;

namespace Hotel.Application.RatePlans.Validators;

public class CreateRatePlanRoomCommandValidator : AbstractValidator<CreateRatePlanRoomCommand>
{
    public CreateRatePlanRoomCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("Room type is required.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must not be negative.");
    }
}