using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using RentACar.Application.Features.Brands.Constants;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Rules;

public class BrandBusinessRules : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository;

    public BrandBusinessRules (IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task BrandNameCannotBeDuplicatedWhenInsertedAsync (string name)
    {
        Brand? result = await _brandRepository.GetAsync(predicate: b => b.BrandName.ToLower() == name.ToLower());

        if (result != null)
        {
            throw new BusinessException(BrandsMessages.BrandNameExists);
        }
    }
}