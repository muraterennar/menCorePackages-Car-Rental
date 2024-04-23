using AutoMapper;
using MediatR;
using MenCore.Application.Request;
using MenCore.Application.Responses;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Queries.GetAll;

public class GetAllOperationClaimQuery : IRequest<GetListResponse<GetAllOperationClaimResponse>>
{
    public PageRequest PageRequest { get; set; }

    public class GetAllOperationClaimQueryHandler : IRequestHandler<GetAllOperationClaimQuery,
        GetListResponse<GetAllOperationClaimResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimService _operationClaimService;

        public GetAllOperationClaimQueryHandler(IMapper mapper, IOperationClaimService operationClaimService)
        {
            _mapper = mapper;
            _operationClaimService = operationClaimService;
        }

        public async Task<GetListResponse<GetAllOperationClaimResponse>> Handle(GetAllOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
            //TODO: Listemede Hata Olabilir index ve size kontrol et
            var operationClaims = await _operationClaimService.GetAllAsync();
            var response = _mapper.Map<GetListResponse<GetAllOperationClaimResponse>>(operationClaims);
            return response;
        }
    }
}