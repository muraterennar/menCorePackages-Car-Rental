using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Logging;
using MenCore.Security.Entities;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.OperationClaims.Queries.GetByName;

public class GetByNameOperationClaimQuery : IRequest<GetByNameOperationClaimResponse>, ILoggableRequest
{
    public string Name { get; set; }

    public GetByNameOperationClaimQuery()
    {
        Name = string.Empty;
    }

    public GetByNameOperationClaimQuery(string name)
    {
        Name = name;
    }

    public class GetByNameOperationClaimQueryHandler : IRequestHandler<GetByNameOperationClaimQuery, GetByNameOperationClaimResponse>
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public GetByNameOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<GetByNameOperationClaimResponse> Handle(GetByNameOperationClaimQuery request, CancellationToken cancellationToken)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(o => o.Name == request.Name);

            GetByNameOperationClaimResponse response = _mapper.Map<GetByNameOperationClaimResponse>(operationClaim);

            return response;
        }
    }
}