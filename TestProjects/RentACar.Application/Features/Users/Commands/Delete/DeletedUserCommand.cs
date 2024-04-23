using AutoMapper;
using MediatR;
using MenCore.Application.Pipelines.Authorization;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using RentACar.Application.Features.Users.Rules;
using RentACar.Application.Services.UserServices;
using static RentACar.Application.Features.Users.Constants.UserOperationClaims;

namespace RentACar.Application.Features.Users.Commands.Delete;

public class DeletedUserCommand : IRequest<DeletedUserResponse>, ITransactionalRequest, ILoggableRequest,
    ISecuredRequest
{
    public DeletedUserCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    public string[] Roles => new[] { Admin, Write };

    public class DeleteUserCommandHandler : IRequestHandler<DeletedUserCommand, DeletedUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userService = userService;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<DeletedUserResponse> Handle(DeletedUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdShouldBeExistWhenSelected(request.Id);

            //User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
            var user = await _userService.GetByIdAsync(request.Id);
            await _userBusinessRules.UserShouldBeExistWhenSelected(user);

            //User deletedUser = await _userRepository.DeleteAsync(user);
            var deletedUser = await _userService.DeleteAsync(user);

            var response = _mapper.Map<DeletedUserResponse>(deletedUser);

            return response;
        }
    }
}