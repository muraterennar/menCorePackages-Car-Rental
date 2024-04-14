using MediatR;
using MenCore.Application.Dtos;
using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using MenCore.Security.JWT;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthServices;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<RegisteredResponse>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IPAddress { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;

        public RegisterCommandHandler(IUserRepository userRepository, IAuthService authService, AuthBusinessRules authBusinessRules)
        {
            _userRepository = userRepository;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RegisteredResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcı e-posta adresinin veritabanında olmadığını kontrol eder
            await _authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);

            // Parola için hash ve salt oluşturur
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);

            // Yeni kullanıcı nesnesi oluşturur
            User? newUser = new()
            {
                Email = request.UserForRegisterDto.Email,
                FirstName = request.UserForRegisterDto.FirstName,
                LastName = request.UserForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };

            // Yeni kullanıcıyı veritabanına ekler
            User? createdUser = await _userRepository.AddAsync(newUser);

            // Yeni kullanıcı için erişim tokenı oluşturur
            AccessToken? createdAccessToken = await _authService.CreateAccessToken(createdUser);

            // Yeni kullanıcı için yenileme tokenı oluşturur ve veritabanına ekler
            RefreshToken? createdResfreshToken = await _authService.CreateRefreshToken(createdUser, request.IPAddress);
            RefreshToken? addedRefreshToken = await _authService.AddRefreshToken(createdResfreshToken);

            // Kayıtlı kullanıcı cevabını oluşturur
            RegisteredResponse? registeredResponse = new() { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };

            return registeredResponse;
        }

    }
}