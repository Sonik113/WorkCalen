using WorkCalendarik.Domain.Database.Entities;

namespace WorkCalendarik.Domain.Validation.Validators;

using FluentValidation;
using WorkCalendarik.Domain.Database.ModelsDb;

public class BronCalendarValidator : AbstractValidator<BronCalendar>
{
    public BronCalendarValidator()
    {
        RuleFor(post => post.Price)
            .GreaterThan(0).WithMessage(ValidationMessages.BronCalendarPricePositive);

        RuleFor(post => post.CreatedAt)
            .NotEmpty().WithMessage(ValidationMessages.BronCalendarCreatedAtRequired);
    }
}
