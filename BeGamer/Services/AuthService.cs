using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Services.Interfaces;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace BeGamer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;
        public AuthService(IJwtTokenService jwtTokenService, IUserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        public string Login(LoginDTO loginDTO)
        {
           
           
            return null;
        }

        public string Register(CreateUserDTO registerUserDTO)
        {
            if (!string.IsNullOrEmpty(registerUserDTO.Username) && !string.IsNullOrEmpty(registerUserDTO.Password))
            {
                _userService.CreateUserAsync(registerUserDTO).Wait();

            }
            // Po úspěšné registraci můžeš rovnou přihlásit uživatele
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, registerUserDTO.Username),
                new Claim(ClaimTypes.Role, "User")
            };
            var token = _jwtTokenService.GenerateToken(claims);
           

                return token;
            
        }
    }
}
