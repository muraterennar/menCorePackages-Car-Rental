using AutoMapper;
using MediatR;
using RentACar.Application.Features.UserOperationClaims.Queries.GetById;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Queries.GetByUserId;

public class GetByUserIdUserOperationClaimQuery : IRequest<GetByIdUserOperationClaimResponse>
{
    public int UserId { get; set; }

    public class GetByUserIdUserOperationClaimQueryHandler : IRequestHandler<GetByUserIdUserOperationClaimQuery,
        GetByIdUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public GetByUserIdUserOperationClaimQueryHandler(IMapper mapper,
            IUserOperationClaimService userOperationClaimService,
            UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<GetByIdUserOperationClaimResponse> Handle(GetByUserIdUserOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.IsUserOperationClaimExistForUser(request.UserId);
            var userOperationClaim = await _userOperationClaimService.GetByUserIdAsync(request.UserId);
            var response = _mapper.Map<GetByIdUserOperationClaimResponse>(userOperationClaim);
            return response;
        }
    }
}