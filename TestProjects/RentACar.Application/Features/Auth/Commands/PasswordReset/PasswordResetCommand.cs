using MediatR;
using MenCore.Application.Extensions;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.PasswordReset;

public class PasswordResetCommand : IRequest<PasswordResetResponse>, ILoggableRequest, ITransactionalRequest
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }

    public class PasswordResetCommandHandler : IRequestHandler<PasswordResetCommand, PasswordResetResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserService _userService;

        public PasswordResetCommandHandler(AuthBusinessRules authBusinessRules, IUserService userService)
        {
            _authBusinessRules = authBusinessRules;
            _userService = userService;
        }

        public async Task<PasswordResetResponse> Handle(PasswordResetCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.IsSystemUser(request.UserId);
            
            await _authBusinessRules.IsTokenValid(request.Token, user.Email);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userService.UpdateAsync(user);

            return new PasswordResetResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }
}