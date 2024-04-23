using AutoMapper;
using MediatR;
using MenCore.Security.Entities;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Commands.Update;

public class UpdateOperationClaimCommands : IRequest<UpdatedOperationClaimResponse>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public class
        UpdateOperationClaimCommandsHandler : IRequestHandler<UpdateOperationClaimCommands,
        UpdatedOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimService _operationClaimService;

        public UpdateOperationClaimCommandsHandler(IMapper mapper, IOperationClaimService operationClaimService)
        {
            _mapper = mapper;
            _operationClaimService = operationClaimService;
        }

        public async Task<UpdatedOperationClaimResponse> Handle(UpdateOperationClaimCommands request,
            CancellationToken cancellationToken)
        {
            var operationClaim = _mapper.Map<OperationClaim>(request);
            var updatedOperationClaim = await _operationClaimService.UpdateAsync(operationClaim);
            var response = _mapper.Map<UpdatedOperationClaimResponse>(updatedOperationClaim);
            return response;
        }
    }
}