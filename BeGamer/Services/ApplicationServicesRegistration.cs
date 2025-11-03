using BeGamer.DTOs.Game;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

public static class ApplicationservicesRegistration
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGameEventService, GameEventService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<AddressService>();

        services.AddScoped<GuidGenerator>();


        services.AddScoped<UserMapper>();
        services.AddScoped<GameMapper>();
        services.AddScoped<GameEventMapper>();

        services.AddScoped<UserManager<CustomUser>>();
        services.AddScoped<PasswordHasher<CustomUser>>();

        return services;
    }
}