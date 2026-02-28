using CarMarketApp.Application.Abstractions.Helpers;
using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Abstractions.Repositories;
using CarMarketApp.Application.Abstractions.Services;
using CarMarketApp.Application.Abstractions.UnitOfWork;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Features.Commands.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Infrastructure.Identity.Entities;
using CarMarketApp.Infrastructure.Implementations.Helpers;
using CarMarketApp.Infrastructure.Implementations.Identity;
using CarMarketApp.Infrastructure.Implementations.Repositories;
using CarMarketApp.Infrastructure.Implementations.Services;
using CarMarketApp.Infrastructure.Implementations.UnitOfWork;
using CarMarketApp.Infrastructure.Mapping;
using CarMarketApp.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarMarketApp.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        // Identity
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            options.Lockout.MaxFailedAccessAttempts = 3;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

        // Repositories
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBrandService, BrandService>();

        // Helpers
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<ITokenHasher, TokenHasher>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>());

        // Fluent Validation
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Behavior
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;

        });

        // 
        // JWT
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtOptions is null)
            throw new InvalidOperationException("JwtSettings section is missing in configuration.");

        if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is missing in configuration.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();

        return services;
    }
}
