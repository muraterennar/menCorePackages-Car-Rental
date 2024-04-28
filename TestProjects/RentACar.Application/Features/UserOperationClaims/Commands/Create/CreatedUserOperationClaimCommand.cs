using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using RentACar.Application.Features.UserOperationClaims.Constants;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.UserOperationClaimServices;
using static MenCore.Security.Constants.GeneralOperationClaims;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Create;

public class CreatedUserOperationClaimCommand: IRequest<CreateUserOperationClaimResponse> ,ITransactionalRequest, ILoggableRequest, ISecuredRequest, ICacheRemoverRequest
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
    public string[] Roles  => new[] {Admin, Write};
    
    public string? CacheKey => String.Empty;
    public bool BypassCache => false;
    public string? CacheGroupKey => UserOperationClaimCacheGroupKey.CacheGroupKey;
    
    public CreatedUserOperationClaimCommand(int userId, int operationClaimId)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
    
    public class  CreatedUserOperationClaimCommandHandler : IRequestHandler<CreatedUserOperationClaimCommand, CreateUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly UserBusinessRules _userBusinessRules;

        public CreatedUserOperationClaimCommandHandler(IMapper mapper, IUserOperationClaimService userOperationClaimService, UserOperationClaimBusinessRules userOperationClaimBusinessRules, UserBusinessRules userBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<CreateUserOperationClaimResponse> Handle(CreatedUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            // User İçinde Girilen Id'nin Varlığını Kontrol Eder.
            await _userBusinessRules.UserIdShouldBeExistWhenSelected(request.UserId);
            
            await _userOperationClaimBusinessRules.IsUserOperationClaimNotExistForUserAndOperationClaim(request.UserId, request.OperationClaimId);
            
            UserOperationClaim userOperationClaim = _mapper.Map<UserOperationClaim>(request);
            
            UserOperationClaim createdUserOperationClaim = await _userOperationClaimService.CreateAsync(userOperationClaim);
            
            CreateUserOperationClaimResponse response = _mapper.Map<CreateUserOperationClaimResponse>(createdUserOperationClaim);

            return response;
        }
    }
}