using FluentValidation;

namespace RentACar.Application.Features.Auth.Commands.VerifyPasswordReset;

public class VerifyPasswordResetCommandValidator:AbstractValidator<VerifyPasswordResetCommand>
{
    public VerifyPasswordResetCommandValidator()
    {
        RuleFor(p=>p.UserId).NotEmpty().NotNull();
        RuleFor(p=>p.Token).NotEmpty().NotNull();
    }
}