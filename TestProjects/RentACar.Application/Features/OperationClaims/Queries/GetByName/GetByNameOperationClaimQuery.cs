using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Logging;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Queries.GetByName;

public class GetByNameOperationClaimQuery : IRequest<GetByNameOperationClaimResponse>, ILoggableRequest
{
    public GetByNameOperationClaimQuery()
    {
        Name = string.Empty;
    }

    public GetByNameOperationClaimQuery(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public class
        GetByNameOperationClaimQueryHandler : IRequestHandler<GetByNameOperationClaimQuery,
        GetByNameOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimService _operationClaimService;

        public GetByNameOperationClaimQueryHandler(IOperationClaimService operationClaimService, IMapper mapper)
        {
            _operationClaimService = operationClaimService;
            _mapper = mapper;
        }

        public async Task<GetByNameOperationClaimResponse> Handle(GetByNameOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
            var operationClaim = await _operationClaimService.GetByNameAsync(request.Name);

            var response = _mapper.Map<GetByNameOperationClaimResponse>(operationClaim);

            return response;
        }
    }
}