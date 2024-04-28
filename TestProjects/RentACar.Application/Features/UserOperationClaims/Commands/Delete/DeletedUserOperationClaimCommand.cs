using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Caching;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using RentACar.Application.Features.UserOperationClaims.Constants;
using RentACar.Application.Features.UserOperationClaims.Rules;
using RentACar.Application.Services.UserOperationClaimServices;

namespace RentACar.Application.Features.UserOperationClaims.Commands.Delete;

public class DeletedUserOperationClaimCommand : IRequest<DeleteUserOperationClaimResponse>, ILoggableRequest, ITransactionalRequest, ICacheRemoverRequest
{
    public int Id { get; set; }
    public int OperationClaimId { get; set; }
    public int UserId { get; set; }
    
    public string? CacheKey => String.Empty;
    public bool BypassCache => false;   
    public string? CacheGroupKey => UserOperationClaimCacheGroupKey.CacheGroupKey;


    public class
        DeletedUserOperationClaimCommandHandler : IRequestHandler<DeletedUserOperationClaimCommand,
        DeleteUserOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

        public DeletedUserOperationClaimCommandHandler(IMapper mapper,
            IUserOperationClaimService userOperationClaimService,
            UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _userOperationClaimService = userOperationClaimService;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<DeleteUserOperationClaimResponse> Handle(DeletedUserOperationClaimCommand request,
            CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.IsUserOperationClaimNotExist(request.Id);

            UserOperationClaim userOperationClaim = _mapper.Map<UserOperationClaim>(request);

            UserOperationClaim deletedUserOperationClaim =
                await _userOperationClaimService.DeleteAsync(userOperationClaim);

            DeleteUserOperationClaimResponse response =
                _mapper.Map<DeleteUserOperationClaimResponse>(deletedUserOperationClaim);

            return response;
        }
    }
}