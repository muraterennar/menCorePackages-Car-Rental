using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using RentACar.Application.Features.Brands.Rules;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<CreateBrandResposne>, ICacheRemoverRequest, ITransactionalRequest, ILoggableRequest
{
    public string BrandName { get; set; }

    public string? CacheGroupKey => "GetBrands";

    public string CacheKey => string.Empty;

    public bool BypassCache => false;

    public class CreatedBrandCommandController : IRequestHandler<CreateBrandCommand, CreateBrandResposne>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreatedBrandCommandController(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<CreateBrandResposne> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInsertedAsync(request.BrandName);

            Brand brand = _mapper.Map<Brand>(request);
            brand.Id = Guid.NewGuid();

            await _brandRepository.AddAsync(brand);

            CreateBrandResposne createBrandResposne = _mapper.Map<CreateBrandResposne>(brand);

            return createBrandResposne;
        }
    }
}