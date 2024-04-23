using AutoMapper;
using MediatR;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Queries.GetByOperationClaimId;

public class GetByOperationClaimIdUserOperationClaimQuery : IRequest<GetByOperationClaimIdUserOperationClaimResponse>
{
    public int OperationClaimId { get; set; }

    public class GetByOperationClaimIdUserOperationClaimQueryHandler : IRequestHandler<
        GetByOperationClaimIdUserOperationClaimQuery, GetByOperationClaimIdUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public GetByOperationClaimIdUserOperationClaimQueryHandler(IMapper mapper,
            IUserOperationClaimService userOperationClaimService,
            UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<GetByOperationClaimIdUserOperationClaimResponse> Handle(
            GetByOperationClaimIdUserOperationClaimQuery request, CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.IsUserOperationClaimExistForOperationClaim(request.OperationClaimId);
            var userOperationClaim =
                await _userOperationClaimService.GetByOperationClaimIdAsync(request.OperationClaimId);
            var response = _mapper.Map<GetByOperationClaimIdUserOperationClaimResponse>(userOperationClaim);
            return response;
        }
    }
}