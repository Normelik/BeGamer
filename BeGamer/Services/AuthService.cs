using BeGamer.DTOs;
using BeGamer.Services.Interfaces;
using System.Security.Claims;

namespace BeGamer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public string Login(string username,
                            string password)
        {
            // Tady bys měl mít reálnou validaci uživatele
            if (username == "admin" && password == "password")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

                var token = _jwtTokenService.GenerateToken(claims);
                return token;
            }
            return null;
        }

        public string Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
