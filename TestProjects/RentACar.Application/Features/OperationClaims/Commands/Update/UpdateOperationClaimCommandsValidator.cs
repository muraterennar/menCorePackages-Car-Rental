using FluentValidation;

namespace RentACar.Application.Features.OperationClaims.Commands.Update;

public class UpdateOperationClaimCommandsValidator : AbstractValidator<UpdateOperationClaimCommands>
{
    public UpdateOperationClaimCommandsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(3).MaximumLength(50);
    }
}