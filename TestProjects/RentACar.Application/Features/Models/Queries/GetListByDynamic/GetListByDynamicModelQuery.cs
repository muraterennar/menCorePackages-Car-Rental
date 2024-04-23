using AutoMapper;
using MediatR;
using MenCore.Application.Request;
using MenCore.Application.Responses;
using MenCore.Persistence.Dynamic;
using Microsoft.EntityFrameworkCore;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Models.Queries.GetListByDynamic;

public class GetListByDynamicModelQuery : IRequest<GetListResponse<GetListByDynamicModelListItemDto>>
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery DynamicQuery { get; set; }

    public class GetListByDynamicModelQueryHandler : IRequestHandler<GetListByDynamicModelQuery,
        GetListResponse<GetListByDynamicModelListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;

        public GetListByDynamicModelQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicModelListItemDto>> Handle(GetListByDynamicModelQuery request,
            CancellationToken cancellationToken)
        {
            var models = await _modelRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                include: m => m.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize
            );

            var response = _mapper.Map<GetListResponse<GetListByDynamicModelListItemDto>>(models);
            return response;
        }
    }
}