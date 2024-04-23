using AutoMapper;
using MenCore.Application.Responses;
using MenCore.Persistence.Paging;
using MenCore.Security.Entities;
using RentACar.Application.Features.OperationClaims.Commands.Create;
using RentACar.Application.Features.OperationClaims.Commands.Delete;
using RentACar.Application.Features.OperationClaims.Commands.Update;
using RentACar.Application.Features.OperationClaims.Queries.GetAll;
using RentACar.Application.Features.OperationClaims.Queries.GetById;
using RentACar.Application.Features.OperationClaims.Queries.GetByName;

namespace RentACar.Application.Features.OperationClaims.Profiles;

public class OperationClaimMappingProfiles : Profile
{
    public OperationClaimMappingProfiles()
    {
        CreateMap<OperationClaim, GetByIdOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetByNameOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetAllOperationClaimResponse>().ReverseMap();
        CreateMap<Paginate<OperationClaim>, GetListResponse<GetAllOperationClaimResponse>>().ReverseMap();

        CreateMap<OperationClaim, CreatedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, CreateOperationClaimCommands>().ReverseMap();

        CreateMap<OperationClaim, UpdateOperationClaimCommands>().ReverseMap();
        CreateMap<OperationClaim, UpdatedOperationClaimResponse>().ReverseMap();

        CreateMap<OperationClaim, DeletedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, DeleteOperationClaimCommands>().ReverseMap();
    }
}