using MediatR;
using MenCore.Application.Extensions;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.VerifyPasswordReset;

public class VerifyPasswordResetCommand:IRequest<VerifyPasswordResetResponse>,ILoggableRequest, ITransactionalRequest
{
    public int UserId { get; set; }
    public string Token { get; set; }
    
    public class  VerifyPasswordResetCommandHandler:IRequestHandler<VerifyPasswordResetCommand, VerifyPasswordResetResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserService _userService;

        public VerifyPasswordResetCommandHandler(AuthBusinessRules authBusinessRules, IUserService userService)
        {
            _authBusinessRules = authBusinessRules;
            _userService = userService;
        }


        public async Task<VerifyPasswordResetResponse> Handle(VerifyPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(request.UserId);
            await _authBusinessRules.IsSystemUser(user.Id);
            await _authBusinessRules.IsTokenValid(request.Token, user.Email);
            return new VerifyPasswordResetResponse
            {
                IsVerified = true
            };
        }
    }
}