using AutoMapper;
using MediatR;
using MenCore.Application.Request;
using MenCore.Application.Responses;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Queries.GetList;

public class GetListUserOperationClaimQuery : IRequest<GetListResponse<GetListUserOperationClaimResponse>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListUserOperationClaimQueryHandler : IRequestHandler<GetListUserOperationClaimQuery,
        GetListResponse<GetListUserOperationClaimResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public GetListUserOperationClaimQueryHandler(IMapper mapper,
            IUserOperationClaimService userOperationClaimService)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
        }

        public async Task<GetListResponse<GetListUserOperationClaimResponse>> Handle(
            GetListUserOperationClaimQuery request, CancellationToken cancellationToken)
        {
            var userOperationClaims = await _userOperationClaimService.IncludableGetAllAsync(request.PageRequest);

            var response = _mapper.Map<GetListResponse<GetListUserOperationClaimResponse>>(userOperationClaims);

            return response;
        }
    }
}