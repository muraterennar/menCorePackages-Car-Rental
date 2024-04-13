using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.Repositories;
using static RentACar.Application.Features.Users.Constants.UserOperationClaims;

namespace RentACar.Application.Features.Users.Commands.Delete;

public class DeletedUserCommand : IRequest<DeletedUserResponse>, ITransactionalRequest, ILoggableRequest, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => new[] { Admin, Write };

    public DeletedUserCommand(int id)
    {
        Id = id;
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeletedUserCommand, DeletedUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<DeletedUserResponse> Handle(DeletedUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdShouldBeExistWhenSelected(request.Id);

            User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
            await _userBusinessRules.UserShouldBeExistWhenSelected(user);

            User deletedUser = await _userRepository.DeleteAsync(user);

            DeletedUserResponse response = _mapper.Map<DeletedUserResponse>(deletedUser);

            return response;
        }
    }
}