using FluentValidation;
using Hotel.Application.Guests.Queries;

namespace Hotel.Application.Guests.Validators;

public class SearchGuestsQueryValidator : AbstractValidator<SearchGuestsQuery>
{
    public SearchGuestsQueryValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.Name) ||
                !string.IsNullOrWhiteSpace(x.Phone) ||
                !string.IsNullOrWhiteSpace(x.Email) ||
                !string.IsNullOrWhiteSpace(x.DocumentNumber))
            .WithMessage("At least one search criterion is required.");
    }
}