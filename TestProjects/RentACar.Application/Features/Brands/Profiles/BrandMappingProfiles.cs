using AutoMapper;
using MenCore.Application.Responses;
using MenCore.Persistence.Paging;
using RentACar.Application.Features.Brands.Commands.Create;
using RentACar.Application.Features.Brands.Commands.Delete;
using RentACar.Application.Features.Brands.Commands.Update;
using RentACar.Application.Features.Brands.Queries.GetById;
using RentACar.Application.Features.Brands.Queries.GetList;
using RentACar.Domaim.Entities;

namespace RentACar.Application.Features.Brands.Profiles;

public class BrandMappingProfiles : Profile
{
    public BrandMappingProfiles()
    {
        CreateMap<Brand, CreateBrandCommand>().ReverseMap();
        CreateMap<Brand, CreateBrandResposne>().ReverseMap();

        CreateMap<Brand, UpdateBrandResponse>().ReverseMap();
        CreateMap<Brand, UpdateBrandCommand>().ReverseMap();

        CreateMap<Brand, DeletedBrandResponse>().ReverseMap();
        CreateMap<Brand, DeletedBrandCommand>().ReverseMap();

        CreateMap<Brand, GetListBrandListItemDto>().ReverseMap();
        CreateMap<Paginate<Brand>, GetListResponse<GetListBrandListItemDto>>().ReverseMap();

        CreateMap<Brand, GetByIdBrandResponse>().ReverseMap();
    }
}