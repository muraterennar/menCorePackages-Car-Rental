using AutoMapper;
using MediatR;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Queries.GetById;

public class GetByIdOperationClaimQuery : IRequest<GetByIdOperationClaimResponse>
{
    public int Id { get; set; }


    public class
        GetByIdOperationClaimQueryHandler : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimService _operationClaimService;

        public GetByIdOperationClaimQueryHandler(IMapper mapper, IOperationClaimService operationClaimService)
        {
            _mapper = mapper;
            _operationClaimService = operationClaimService;
        }

        public async Task<GetByIdOperationClaimResponse> Handle(GetByIdOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
            var operationClaim = await _operationClaimService.GetByIdAsync(request.Id);
            var response = _mapper.Map<GetByIdOperationClaimResponse>(operationClaim);
            return response;
        }
    }
}