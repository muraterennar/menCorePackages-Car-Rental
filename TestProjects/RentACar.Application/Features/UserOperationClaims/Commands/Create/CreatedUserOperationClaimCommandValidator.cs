using FluentValidation;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Create;

public class CreatedUserOperationClaimCommandValidator:AbstractValidator<CreatedUserOperationClaimCommand>
{
    public CreatedUserOperationClaimCommandValidator()
    {
        RuleFor(u=>u.UserId).NotEmpty().NotNull();
        RuleFor(u=>u.OperationClaimId).NotEmpty().NotNull();
    }
}