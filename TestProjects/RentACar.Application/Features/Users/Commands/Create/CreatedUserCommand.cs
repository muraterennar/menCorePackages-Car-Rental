using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;
using static MenCore.Security.Constants.GeneralOperationClaims;

namespace RentACar.Application.Features.Users.Commands.Create;

public class CreatedUserCommand : IRequest<CreatedUserResponse>, ISecuredRequest, ITransactionalRequest
{
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

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string[] Roles => new[] { Admin, Write };

    public class CreateUserCommandHandler : IRequestHandler<CreatedUserCommand, CreatedUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IMapper mapper,
            UserBusinessRules userBusinessRules, IUserService userService)
        {
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
            _userService = userService;
        }

        public async Task<CreatedUserResponse> Handle(CreatedUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserEmailShouldNotExistWhenInsert(request.Email);

            var user = _mapper.Map<User>(request);

            HashingHelper.CreatePasswordHash(
                request.Password,
                out var passwordHash,
                out var passwordSalt
            );
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var createdUser = await _userService.CreateAsync(user);

            var response = _mapper.Map<CreatedUserResponse>(createdUser);

            return response;
        }
    }
}