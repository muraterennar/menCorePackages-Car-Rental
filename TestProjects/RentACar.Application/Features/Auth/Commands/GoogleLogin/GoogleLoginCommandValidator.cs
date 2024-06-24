using FluentValidation;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Auth.Commands.GoogleLogin;

public class GoogleLoginCommandValidator : AbstractValidator<UserLogin>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(u => u.LoginProvider).NotEmpty().NotNull();
        RuleFor(u => u.ProviderKey).NotEmpty().NotNull();
        RuleFor(u => u.UserId).NotEmpty().NotNull();
    }
}