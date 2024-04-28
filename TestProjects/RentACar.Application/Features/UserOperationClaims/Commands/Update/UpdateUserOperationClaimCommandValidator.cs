using FluentValidation;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimCommandValidator:AbstractValidator<UpdatedUserOperationClaimCommand>
{
    public UpdateUserOperationClaimCommandValidator()
    {
        RuleFor(u=>u.Id).NotEmpty().NotNull();
        RuleFor(u=>u.UserId).NotEmpty().NotNull();
        RuleFor(u=>u.OperationClaimId).NotEmpty().NotNull();
    }
}