using FluentValidation;

namespace RentACar.Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimCommandsValidator : AbstractValidator<CreateOperationClaimCommands>
{
    public CreateOperationClaimCommandsValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}