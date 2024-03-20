using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Request;
using MenCore.Application.Responses;
using MenCore.Persistence.Paging;
using RentACar.Application.Services.Repositories;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Queries.GetList;

public class GetListBrandQuery : IRequest<GetListResponse<GetListBrandListItemDto>>, ICachableRequest, ILoggableRequest
{
    public PageRequest PageRequest { get; set; }

    public string CacheGroupKey => "GetBrands";

    public string CacheKey => $"GetListBrandQuery({PageRequest.PageIndex}, {PageRequest.PageSize})";

    public bool BypassCache { get; }

    public TimeSpan? SlidingExpiration { get; }

    public class GetListBrandQueryHandler : IRequestHandler<GetListBrandQuery, GetListResponse<GetListBrandListItemDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBrandListItemDto>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
        {
            Paginate<Brand> brands = await _brandRepository.GetListAsync(
                 index: request.PageRequest.PageIndex,
                 size: request.PageRequest.PageSize,
                 cancellationToken: cancellationToken
                 );

            GetListResponse<GetListBrandListItemDto> response = _mapper.Map<GetListResponse<GetListBrandListItemDto>>(brands);

            return response;
        }
    }
}