using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Brands.Commands.Update;

public class UpdateBrandCommand : IRequest<UpdateBrandResponse>
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>,
        ICacheRemoverRequest
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public string? CacheKey => string.Empty;

        public bool BypassCache => false;

        public string? CacheGroupKey => "GetBrands";

        public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(b => b.Id == request.Id);

            brand = _mapper.Map(request, brand);

            var updatedBrand = await _brandRepository.UpdateAsync(brand);

            var resposne = _mapper.Map<UpdateBrandResponse>(updatedBrand);
            return resposne;
        }
    }
}