using FluentValidation;

namespace RentACar.Application.Features.Users.Commands.Create;

public class CreatedUserCommandValidator:AbstractValidator<CreatedUserCommand>
{
	public CreatedUserCommandValidator()
	{
		RuleFor(u => u.FirstName).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
		RuleFor(u => u.LastName).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
		RuleFor(u => u.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(12);
	}
}