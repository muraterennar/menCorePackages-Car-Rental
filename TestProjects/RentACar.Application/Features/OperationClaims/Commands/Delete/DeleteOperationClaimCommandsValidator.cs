using FluentValidation;

namespace RentACar.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimCommandsValidator : AbstractValidator<DeleteOperationClaimCommands>
{
    public DeleteOperationClaimCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}