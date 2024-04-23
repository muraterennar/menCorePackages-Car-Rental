using AutoMapper;
using MediatR;
using RentACar.Application.Services.OperationClaimServices;

namespace RentACar.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimCommands : IRequest<DeletedOperationClaimResponse>
{
    public int Id { get; set; }

    public class
        DeleteOperationClaimCommandsHandler : IRequestHandler<DeleteOperationClaimCommands,
        DeletedOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOperationClaimService _operationClaimService;

        public DeleteOperationClaimCommandsHandler(IMapper mapper, IOperationClaimService operationClaimService)
        {
            _mapper = mapper;
            _operationClaimService = operationClaimService;
        }

        public async Task<DeletedOperationClaimResponse> Handle(DeleteOperationClaimCommands request,
            CancellationToken cancellationToken)
        {
            var operationClaim = await _operationClaimService.GetByIdAsync(request.Id);
            operationClaim = _mapper.Map(request, operationClaim);
            await _operationClaimService.DeleteAsync(operationClaim);
            var response = _mapper.Map<DeletedOperationClaimResponse>(operationClaim);
            return response;
        }
    }
}