using AutoMapper;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Infrastructure.Identity.Entities;

namespace CarMarketApp.Infrastructure.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, AppUser>();
    }
}
