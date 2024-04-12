using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.Repositories;
using static RentACar.Application.Features.Users.Constants.UserOperationClaims;

namespace RentACar.Application.Features.Users.Commands.Create;

public class CreatedUserCommand : IRequest<CreatedUserResponse>, ISecuredRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string[] Roles => new[] { Admin, Write, Add };

    public CreatedUserCommand()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public CreatedUserCommand(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public class CreateUserCommandHandler : IRequestHandler<CreatedUserCommand, CreatedUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<CreatedUserResponse> Handle(CreatedUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserEmailShouldNotExistWhenInsert(request.Email);

            User user = _mapper.Map<User>(request);

            HashingHelper.CreatePasswordHash(
                    request.Password,
                    passwordHash: out byte[] passwordHash,
                    passwordSalt: out byte[] passwordSalt
                );
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            User createdUser = await _userRepository.AddAsync(user);

            CreatedUserResponse response = _mapper.Map<CreatedUserResponse>(createdUser);

            return response;
        }
    }
}