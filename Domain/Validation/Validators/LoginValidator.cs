using FluentValidation;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.ViewModels.LogAndReg;

namespace WorkCalendarik.Domain.Validation.Validators;

using FluentValidation;

public class LoginValidator : AbstractValidator<LoginViewModel>
{
    public LoginValidator()
    {
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired)
            .MinimumLength(6).WithMessage(ValidationMessages.UserPasswordLength)
            .Matches(RegexPatterns.PasswordRegex).WithMessage(ValidationMessages.PasswordInvalid);

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(ValidationMessages.UserEmailRequired)
            .Matches(RegexPatterns.EmailRegex).WithMessage(ValidationMessages.UserEmailInvalid);
    }
}