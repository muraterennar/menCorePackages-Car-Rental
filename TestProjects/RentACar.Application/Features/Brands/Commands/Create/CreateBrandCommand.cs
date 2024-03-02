using AutoMapper;
using MediatR;
using RentACar.Application.Services.Repositories.BrandRepositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<CreateBrandResposne>
{
    public string Name { get; set; }

    public class CreatedBrandCommandController : IRequestHandler<CreateBrandCommand, CreateBrandResposne>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public CreatedBrandCommandController (IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<CreateBrandResposne> Handle (CreateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = _mapper.Map<Brand>(request);
            brand.Id = Guid.NewGuid();

            await _brandRepository.AddAsync(brand);

            CreateBrandResposne createBrandResposne = _mapper.Map<CreateBrandResposne>(brand);

            return createBrandResposne;
        }
    }
}