using FluentValidation;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(c => c.BrandName).NotEmpty().MinimumLength(2).MaximumLength(50);
    }
}