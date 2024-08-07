using FluentValidation;

namespace RentACar.Application.Features.Auth.Commands.SendPasswordResetLink;

public class SendPasswordResetLinkCommandValidator:AbstractValidator<SendPasswordResetLinkCommand>
{
    public SendPasswordResetLinkCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
    }
}