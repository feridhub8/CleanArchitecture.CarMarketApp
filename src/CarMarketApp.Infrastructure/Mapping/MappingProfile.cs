using AutoMapper;
using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Infrastructure.Identity.Entities;

namespace CarMarketApp.Infrastructure.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, AppUser>();

        CreateMap<AppUser, UserClaimsDto>();

        CreateMap<UpdateUserDto, AppUser>();

        CreateMap<AppUser, UserDto>();
    }
}
