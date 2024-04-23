using AutoMapper;
using MenCore.Application.Responses;
using MenCore.Persistence.Paging;
using RentACar.Application.Features.Models.Queries.GetList;
using RentACar.Application.Features.Models.Queries.GetListByDynamic;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Model, GetListModelListItemDto>()
            .ForMember(b => b.BrandName, opt => opt.MapFrom(b => b.Brand.BrandName))
            .ForMember(b => b.FuelName, opt => opt.MapFrom(b => b.Fuel.FuelName))
            .ForMember(b => b.TransmissionName, opt => opt.MapFrom(b => b.Transmission.TransmissionName))
            .ReverseMap();

        CreateMap<Model, GetListByDynamicModelListItemDto>()
            .ForMember(b => b.BrandName, opt => opt.MapFrom(b => b.Brand.BrandName))
            .ForMember(b => b.FuelName, opt => opt.MapFrom(b => b.Fuel.FuelName))
            .ForMember(b => b.TransmissionName, opt => opt.MapFrom(b => b.Transmission.TransmissionName))
            .ReverseMap();

        CreateMap<Paginate<Model>, GetListResponse<GetListModelListItemDto>>().ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();
    }
}