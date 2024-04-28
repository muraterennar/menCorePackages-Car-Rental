using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using MenCore.Security.Enums;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Features.Users.Constants;

namespace RentACar.Application.Features.Users.Commands.Update;

public class UpdatedUserCommand : IRequest<UpdatedUserResponse>, ITransactionalRequest, ILoggableRequest,
    ISecuredRequest
{
    public UpdatedUserCommand()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Username = string.Empty;
        FullName = string.Empty;
        IdentityNumber = string.Empty;
        Email = string.Empty;
        Status = true;
    }

    public UpdatedUserCommand(string firstName, string lastName, string? fullName, string username,
        string? identityNumber, short? birthYear, string email, bool status, AuthenticatorType authenticatorType)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = fullName;
        Username = username;
        IdentityNumber = identityNumber;
        BirthYear = birthYear;
        Email = email;
        Status = status;
        AuthenticatorType = authenticatorType;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? FullName { get; set; }
    public string Username { get; set; }
    public string? IdentityNumber { get; set; }
    public short? BirthYear { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }

    public string[] Roles => new[] {UserOperationsClaims.Admin, UserOperationsClaims.Write, UserOperationsClaims.UserWrite };

    public class UpdateUserCommandHandler : IRequestHandler<UpdatedUserCommand, UpdatedUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
            UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<UpdatedUserResponse> Handle(UpdatedUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            await _userBusinessRules.UserEmailShouldNotExistWhenInsert(request.Email);
            await _userBusinessRules.UserShouldBeExistWhenSelected(user);
            await _userBusinessRules.UserIdShouldBeExistWhenSelected(request.Id);

            var updatedUser = await _userRepository.UpdateAsync(user);

            var response = _mapper.Map<UpdatedUserResponse>(updatedUser);

            return response;
        }
    }
}