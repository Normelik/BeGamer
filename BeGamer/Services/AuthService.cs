using BeGamer.DTOs;
using BeGamer.DTOs.Auth;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Security.Claims;

namespace BeGamer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IJwtTokenService jwtTokenService,
                            IUserService userService,
                            UserManager<CustomUser> userManager,
                            ILogger<AuthService> logger)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ResponseTokenDTO?> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userService.GetUserByUsernameAsync(loginDTO.Username);

            // prevence for user enumeration
            if (user is null)
            {
                user = new CustomUser { UserName = "Dummy" };
                await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (result is false)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDTO.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id ),
            };

            var token = _jwtTokenService.GenerateToken(claims);

            return new ResponseTokenDTO(token);
        }

        public async Task<string> RegisterAsync(RegisterUserDTO registerUserDTO)
        {
            _logger.LogInformation("Registration attempt for username: {Username}", registerUserDTO.Username);

            try
            {
                var existingUser = await _userService.GetUserByUsernameAsync(registerUserDTO.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Username {Username} already exists.", registerUserDTO.Username);
                    throw new InvalidOperationException("Username already exists.");
                }

                var user = await _userService.CreateUserAsync(registerUserDTO);

                if (user == null)
                {
                    _logger.LogWarning("User creation failed for username: {Username}", registerUserDTO.Username);
                    throw new InvalidOperationException("User could not be created.");
                }

                _logger.LogInformation("User {Username} registered successfully.", registerUserDTO.Username);
                return "User registered successfully.";
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error while registering user: {Username}", registerUserDTO.Username);
                throw; 
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error while registering user: {Username}", registerUserDTO.Username);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while registering user: {Username}", registerUserDTO.Username);
                throw;
            }
        }
    }
}
