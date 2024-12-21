using FluentValidation;
using WorkCalendarik.Domain.Database.Entities;
using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.ViewModels.LogAndReg;

namespace WorkCalendarik.Domain.Validation.Validators;

using FluentValidation;

public class RegisterValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterValidator()
    {
        RuleFor(user => user.Login)
            .NotEmpty().WithMessage(ValidationMessages.UserLoginRequired)
            .MaximumLength(50).WithMessage(ValidationMessages.UserLoginLength)
            .Matches(RegexPatterns.LoginRegex).WithMessage(ValidationMessages.LoginInvalid);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired)
            .MinimumLength(6).WithMessage(ValidationMessages.UserPasswordLength)
            .Matches(RegexPatterns.PasswordRegex).WithMessage(ValidationMessages.PasswordInvalid);

        RuleFor(user => user.PasswordConfirm)
            .NotEmpty().WithMessage(ValidationMessages.UserPasswordRequired)
            .Equal(user => user.Password).WithMessage(ValidationMessages.PasswordMismatch);

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(ValidationMessages.UserEmailRequired)
            .Matches(RegexPatterns.EmailRegex).WithMessage(ValidationMessages.UserEmailInvalid);
    }
}