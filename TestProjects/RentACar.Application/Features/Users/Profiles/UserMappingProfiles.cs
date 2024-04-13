using AutoMapper;
using MenCore.Security.Entities;
using RentACar.Application.Features.Users.Commands.Create;
using RentACar.Application.Features.Users.Commands.Delete;
using RentACar.Application.Features.Users.Commands.Update;

namespace RentACar.Application.Features.Users.Profiles;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<User, CreatedUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();

        CreateMap<User, UpdatedUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();

        CreateMap<User, DeletedUserCommand>().ReverseMap();
        CreateMap<User, DeletedUserResponse>().ReverseMap();
    }
}