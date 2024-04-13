using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Logging;
using MenCore.Security.Entities;
using MenCore.Security.JWT;
using Microsoft.Extensions.Configuration;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.Repositories;
using static RentACar.Application.Features.Users.Constants.UserOperationClaims;

namespace RentACar.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>, ILoggableRequest
{
    public string? EmailOrUsername { get; set; }
    public string Password { get; set; }

    public LoginCommand()
    {
        EmailOrUsername = string.Empty;
        Password = string.Empty;
    }

    public LoginCommand(string? emailOrUsername, string password)
    {
        EmailOrUsername = emailOrUsername;
        Password = password;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IUserRepository userRepository, IMapper mapper, LoginBusinessRules loginBusinessRules, IConfiguration configuration, IUserOperationClaimRepository userOperationClaimRepository, IOperationClaimRepository operationClaimRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _loginBusinessRules = loginBusinessRules;
            _configuration = configuration;
            _userOperationClaimRepository = userOperationClaimRepository;
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.Username == request.EmailOrUsername);

            List<OperationClaim> operationClaims = new();
            OperationClaim? adminClaim = await _operationClaimRepository.GetAsync(o => o.Name == Admin);
            OperationClaim? writerCliam = await _operationClaimRepository.GetAsync(o => o.Name == Write);

            operationClaims.Add(adminClaim);
            operationClaims.Add(writerCliam);

            if (user == null)
            {
                await _loginBusinessRules.UserEmailShouldBeMatched(request.EmailOrUsername);
                user = await _userRepository.GetAsync(u => u.Email == request.EmailOrUsername);
            }
            else
            {
                await _loginBusinessRules.UserUsernameShouldBeMatched(request.EmailOrUsername);
            }

            await _loginBusinessRules.IsPasswordVerified(user, request.Password);

            JwtHelper jwtHelper = new(_configuration);
            AccessToken accessToken = jwtHelper.CreateToken(user, operationClaims);

            return new LoginResponse
            {
                Token = accessToken.Token,
                ExpirationDate = accessToken.Expiration
            };
        }
    }
}