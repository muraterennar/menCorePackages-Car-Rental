using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Security.Entities;
using RentACar.Application.Features.UserOperationClaims.Constants;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Update;

public class UpdatedUserOperationClaimCommand:IRequest<UpdateUserOperationClaimResponse>, ILoggableRequest, ICacheRemoverRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
    
    public string? CacheKey => String.Empty;
    public bool BypassCache => false;
    public string? CacheGroupKey => UserOperationClaimCacheGroupKey.CacheGroupKey;


    public class UpdatedUserOperationClaimCommandHandler : IRequestHandler<UpdatedUserOperationClaimCommand,UpdateUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

        public UpdatedUserOperationClaimCommandHandler(IMapper mapper, IUserOperationClaimService userOperationClaimService, UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<UpdateUserOperationClaimResponse> Handle(UpdatedUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.IsUserOperationClaimExist(request.Id);
            
            await _userOperationClaimBusinessRules.IsUserOperationClaimNotExistForUserAndOperationClaim(request.UserId, request.OperationClaimId);
            
            UserOperationClaim userOperationClaim = _mapper.Map<UserOperationClaim>(request);
            
            UserOperationClaim updatedUserOperationClaim = await _userOperationClaimService.UpdateAsync(userOperationClaim);
            
            UpdateUserOperationClaimResponse response = _mapper.Map<UpdateUserOperationClaimResponse>(updatedUserOperationClaim);
            
            return response;
        }
    }
}