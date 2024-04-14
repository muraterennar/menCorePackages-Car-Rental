﻿using AutoMapper;
using MediatR;
using MenCore.Security.Entities;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthServices;

namespace RentACar.Application.Features.Auth.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<RevokedTokenResponse>
{
    public string Token { get; set; }
    public string IPAddress { get; set; }

    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, RevokedTokenResponse>
    {
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IMapper _mapper;

        public RevokeTokenCommandHandler(IAuthService authService, AuthBusinessRules authBusinessRules, IMapper mapper)
        {
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _mapper = mapper;
        }

        public async Task<RevokedTokenResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            // Yenileme tokenını alır
            RefreshToken? refreshToken = await _authService.GetRefreshTokenByToken(request.Token);

            // Yenileme tokenının varlığını kontrol eder
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            // Yenileme tokenının geçerliliğini kontrol eder
            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            // Yenileme tokenını iptal eder
            await _authService.RevokeRefreshToken(refreshToken, request.IPAddress, reason: "Revoked without replacement.");

            // İptal edilmiş token cevabını oluşturur
            RevokedTokenResponse revokedTokenResponse = _mapper.Map<RevokedTokenResponse>(refreshToken);

            return revokedTokenResponse;
        }

    }
}