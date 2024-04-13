using AutoMapper;
using MenCore.Security.Entities;
using RentACar.Application.Features.OperationClaims.Queries.GetByName;

namespace RentACar.Application.Features.OperationClaims.Rules;

public class OperationClaimBusinessRules : Profile
{
    public OperationClaimBusinessRules()
    {
        CreateMap<GetByNameOperationClaimQuery, OperationClaim>().ReverseMap();
        CreateMap<GetByNameOperationClaimResponse, OperationClaim>().ReverseMap();
    }
}