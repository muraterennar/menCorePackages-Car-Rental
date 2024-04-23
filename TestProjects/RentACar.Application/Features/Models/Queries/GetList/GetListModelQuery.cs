using AutoMapper;
using MediatR;
using MenCore.Application.Request;
using MenCore.Application.Responses;
using Microsoft.EntityFrameworkCore;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Models.Queries.GetList;

public class GetListModelQuery : IRequest<GetListResponse<GetListModelListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetLisModelQueryHandler : IRequestHandler<GetListModelQuery, GetListResponse<GetListModelListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;

        public GetLisModelQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListModelListItemDto>> Handle(GetListModelQuery request,
            CancellationToken cancellationToken)
        {
            var models = await _modelRepository.GetListAsync(
                include: m => m.Include(m => m.Brand).Include(m => m.Transmission).Include(m => m.Fuel),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize
            );

            var response = _mapper.Map<GetListResponse<GetListModelListItemDto>>(models);

            return response;
        }
    }
}