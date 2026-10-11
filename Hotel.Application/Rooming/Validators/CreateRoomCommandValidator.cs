using FluentValidation;
using Hotel.Application.Rooming.Commands;

namespace Hotel.Application.Rooming.Validators;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Room number is required.")
            .MaximumLength(20).WithMessage("Room number must not exceed 20 characters.");

        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("Room type is required.");
    }
}