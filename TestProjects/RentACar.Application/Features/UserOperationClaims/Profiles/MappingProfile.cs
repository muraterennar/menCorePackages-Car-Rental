using AutoMapper;
using MenCore.Application.Responses;
using MenCore.Persistence.Paging;
using MenCore.Security.Entities;
using RentACar.Application.Features.UserOperationClaims.Queries.GetById;
using RentACar.Application.Features.UserOperationClaims.Queries.GetByOperationClaimId;
using RentACar.Application.Features.UserOperationClaims.Queries.GetByUserId;
using RentACar.Application.Features.UserOperationClaims.Queries.GetList;

namespace RentACar.Application.Features.UserOperationClaims.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureMapping(CreateMap<UserOperationClaim, GetListUserOperationClaimResponse>()).ReverseMap();

        CreateMap<Paginate<UserOperationClaim>, GetListResponse<GetListUserOperationClaimResponse>>().ReverseMap();

        ConfigureMapping(CreateMap<UserOperationClaim, GetByIdUserOperationClaimResponse>()).ReverseMap();

        ConfigureMapping(CreateMap<UserOperationClaim, GetByUserIdUserOperationClaimResponse>()).ReverseMap();

        ConfigureMapping(CreateMap<UserOperationClaim, GetByOperationClaimIdUserOperationClaimResponse>()).ReverseMap();
    }

    // GetListUserOperationClaimResponse
    private IMappingExpression<UserOperationClaim, GetListUserOperationClaimResponse> ConfigureMapping(
        IMappingExpression<UserOperationClaim, GetListUserOperationClaimResponse> map)
    {
        return map
            .ForMember(u => u.OperationClaimName,
                opt => opt.MapFrom(u => u.OperationClaim.Name))
            .ForMember(u => u.UserId,
                opt => opt.MapFrom(u => u.User.Id))
            .ForMember(u => u.OpeartionClaimId,
                opt => opt.MapFrom(u => u.OperationClaim.Id))
            .ForMember(u => u.Username,
                opt => opt.MapFrom(u => u.User.Username))
            .ForMember(u => u.FirstName,
                opt => opt.MapFrom(u => u.User.FirstName))
            .ForMember(u => u.LastName,
                opt => opt.MapFrom(u => u.User.LastName))
            .ForMember(u => u.Email,
                opt => opt.MapFrom(u => u.User.Email));
    }

    // GetByIdUserOperationClaimResponse
    private IMappingExpression<UserOperationClaim, GetByIdUserOperationClaimResponse> ConfigureMapping(
        IMappingExpression<UserOperationClaim, GetByIdUserOperationClaimResponse> map)
    {
        return map
            .ForMember(u => u.OperationClaimName,
                opt => opt.MapFrom(u => u.OperationClaim.Name))
            .ForMember(u => u.UserId,
                opt => opt.MapFrom(u => u.User.Id))
            .ForMember(u => u.OpeartionClaimId,
                opt => opt.MapFrom(u => u.OperationClaim.Id))
            .ForMember(u => u.Username,
                opt => opt.MapFrom(u => u.User.Username))
            .ForMember(u => u.FirstName,
                opt => opt.MapFrom(u => u.User.FirstName))
            .ForMember(u => u.LastName,
                opt => opt.MapFrom(u => u.User.LastName))
            .ForMember(u => u.Email,
                opt => opt.MapFrom(u => u.User.Email));
    }

    // GetByUserIdUserOperationClaimResponse
    private IMappingExpression<UserOperationClaim, GetByUserIdUserOperationClaimResponse> ConfigureMapping(
        IMappingExpression<UserOperationClaim, GetByUserIdUserOperationClaimResponse> map)
    {
        return map
            .ForMember(u => u.OperationClaimName,
                opt => opt.MapFrom(u => u.OperationClaim.Name))
            .ForMember(u => u.UserId,
                opt => opt.MapFrom(u => u.User.Id))
            .ForMember(u => u.OpeartionClaimId,
                opt => opt.MapFrom(u => u.OperationClaim.Id))
            .ForMember(u => u.Username,
                opt => opt.MapFrom(u => u.User.Username))
            .ForMember(u => u.FirstName,
                opt => opt.MapFrom(u => u.User.FirstName))
            .ForMember(u => u.LastName,
                opt => opt.MapFrom(u => u.User.LastName))
            .ForMember(u => u.Email,
                opt => opt.MapFrom(u => u.User.Email));
    }

    // GetByOperationClaimIdUserOperationClaimResponse
    private IMappingExpression<UserOperationClaim, GetByOperationClaimIdUserOperationClaimResponse> ConfigureMapping(
        IMappingExpression<UserOperationClaim, GetByOperationClaimIdUserOperationClaimResponse> map)
    {
        return map
            .ForMember(u => u.OperationClaimName,
                opt => opt.MapFrom(u => u.OperationClaim.Name))
            .ForMember(u => u.UserId,
                opt => opt.MapFrom(u => u.User.Id))
            .ForMember(u => u.OpeartionClaimId,
                opt => opt.MapFrom(u => u.OperationClaim.Id))
            .ForMember(u => u.Username,
                opt => opt.MapFrom(u => u.User.Username))
            .ForMember(u => u.FirstName,
                opt => opt.MapFrom(u => u.User.FirstName))
            .ForMember(u => u.LastName,
                opt => opt.MapFrom(u => u.User.LastName))
            .ForMember(u => u.Email,
                opt => opt.MapFrom(u => u.User.Email));
    }
}