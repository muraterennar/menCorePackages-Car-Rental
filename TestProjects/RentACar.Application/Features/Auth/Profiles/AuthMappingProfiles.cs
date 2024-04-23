using AutoMapper;
using MenCore.Security.Entities;
using RentACar.Application.Features.Auth.Commands.RevokeToken;

namespace RentACar.Application.Features.Auth.Profiles;

public class AuthMappingProfiles : Profile
{
    public AuthMappingProfiles()
    {
        CreateMap<RefreshToken, RevokedTokenResponse>().ReverseMap();
    }
}