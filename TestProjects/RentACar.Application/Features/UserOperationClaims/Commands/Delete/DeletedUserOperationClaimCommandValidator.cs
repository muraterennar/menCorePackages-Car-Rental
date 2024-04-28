using FluentValidation;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Delete;

public class DeletedUserOperationClaimCommandValidator:AbstractValidator<DeletedUserOperationClaimCommand>
{
    public DeletedUserOperationClaimCommandValidator()
    {
        RuleFor(u=>u.Id).NotEmpty().NotNull();
    }
}