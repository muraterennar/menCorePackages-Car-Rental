using AutoMapper;
using MediatR;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Brands.Queries.GetById;

public class GetByIdBrendQuery : IRequest<GetByIdBrandResponse>
{
    public Guid Id { get; set; }

    public class GetByIdQueryHandler : IRequestHandler<GetByIdBrendQuery, GetByIdBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdBrandResponse> Handle(GetByIdBrendQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(
                b => b.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var response = _mapper.Map<GetByIdBrandResponse>(brand);

            return response;
        }
    }
}