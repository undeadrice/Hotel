using FluentValidation;
using Hotel.Application.Rooming.Queries;

namespace Hotel.Application.Rooming.Validators;

public class GetRoomByIdQueryValidator : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room id is required.");
    }
}