using AutoMapper;
using MediatR;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Queries.GetById;

public class GetByIdUserOperationClaimQuery : IRequest<GetByIdUserOperationClaimResponse>
{
    public int Id { get; set; }

    public class
        GetByIdUserOperationClaimQueryHandler : IRequestHandler<GetByIdUserOperationClaimQuery,
        GetByIdUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public GetByIdUserOperationClaimQueryHandler(IMapper mapper,
            IUserOperationClaimService userOperationClaimService,
            UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<GetByIdUserOperationClaimResponse> Handle(GetByIdUserOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.IsUserOperationClaimExist(request.Id);

            var getUserOperationClaim = await _userOperationClaimService.GetByIdAsync(request.Id);
            var response =
                _mapper.Map<GetByIdUserOperationClaimResponse>(getUserOperationClaim);
            return response;
        }
    }
}