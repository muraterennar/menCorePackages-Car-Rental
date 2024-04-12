using AutoMapper;
using MenCore.Security.Entities;
using RentACar.Application.Features.Users.Commands.Create;

namespace RentACar.Application.Features.Users.Profiles;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<User, CreatedUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();
    }
}