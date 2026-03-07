using AutoMapper;
using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Domain.Entities;
using CarMarketApp.Infrastructure.Identity.Entities;

namespace CarMarketApp.Infrastructure.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<RegisterUserDto, AppUser>();

        CreateMap<AppUser, UserClaimsDto>();

        CreateMap<UpdateUserDto, AppUser>();

        CreateMap<AppUser, UserDto>()
            .ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role!.Name))
            );

        // Brand
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<Brand, BrandDto>();

        // Model
        CreateMap<CreateModelDto, Model>();
        CreateMap<Model, ModelDto>()
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand!.Name));

        // Advert
        CreateMap<CreateAdvertDto, Advert>();
        CreateMap<UpdateAdvertDto, Advert>();
        CreateMap<Advert, AdvertDto>()
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Model.Brand!.Name));

    }
}
