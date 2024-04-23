using AutoMapper;
using MediatR;
using MenCore.Security.Entities;
using RentACar.Application.Features.OperationClaims.Rules;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimCommands : IRequest<CreatedOperationClaimResponse>
{
    public string Name { get; set; }

    public class
        CreateOperationClaimCommandsHandler : IRequestHandler<CreateOperationClaimCommands,
        CreatedOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly OperationClaimBusinessRules _operationClaimBusinessRules;
        private readonly IOperationClaimService _operationClaimService;

        public CreateOperationClaimCommandsHandler(IMapper mapper, IOperationClaimService operationClaimService,
            OperationClaimBusinessRules operationClaimBusinessRules)
        {
            _mapper = mapper;
            _operationClaimService = operationClaimService;
            _operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<CreatedOperationClaimResponse> Handle(CreateOperationClaimCommands request,
            CancellationToken cancellationToken)
        {
            await _operationClaimBusinessRules.IsOperationClaimNotFound(request.Name);
            await _operationClaimBusinessRules.IsOperationClaimExist(request.Name);

            var operationClaim = _mapper.Map<OperationClaim>(request);
            var createdOperationClaim = await _operationClaimService.CreateAsync(operationClaim);
            var response = _mapper.Map<CreatedOperationClaimResponse>(createdOperationClaim);
            return response;
        }
    }
}