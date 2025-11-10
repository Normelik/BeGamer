using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Impl;
using BeGamer.Repositories.Interfaces;
using BeGamer.Services;
using BeGamer.Services.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly)
    {

        // SERVICES
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IJwtTokenService,JwtTokenService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IGameEventService,GameEventService>();
        services.AddScoped<IGameService,GameService>();
        services.AddScoped<IAddressService,AddressService>();

        //REPOSITORIES

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGameEventRepository, GameEventRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        // UTILS
        services.AddScoped<GuidGenerator>();


        // MAPPERS
        services.AddScoped<GenericMapper>();

        // IDENTITY
        services.AddScoped<UserManager<CustomUser>>();
        services.AddScoped<PasswordHasher<CustomUser>>();

        return services;
    }
}