using FluentValidation;
using Hotel.Application.Rooming.Commands;

namespace Hotel.Application.Rooming.Validators;

public class DeactivateRoomCommandValidator : AbstractValidator<DeactivateRoomCommand>
{
    public DeactivateRoomCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Room id is required.");
    }
}