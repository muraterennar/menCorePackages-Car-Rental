using FluentValidation;

namespace RentACar.Application.Features.Auth.Commands.PasswordReset;

public class PasswordResetCommandValidator:AbstractValidator<PasswordResetCommand>
{
    public PasswordResetCommandValidator()
    {
        RuleFor(p => p.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(p => p.Token)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(p => p.NewPassword)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
}