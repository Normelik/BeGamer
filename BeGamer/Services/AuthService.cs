using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeGamer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly PasswordHasher<CustomUser> _passwordHasher;

        public AuthService(IJwtTokenService jwtTokenService, IUserService userService, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, PasswordHasher<CustomUser> passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {

            var user = await _userService.GetUserByUsernameAsync(loginDTO.Username);

            if (user == null)
            {
                // user not found
                throw new Exception("User not found");
            }

            // verify password
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);

            if (!result.Succeeded)
            {
                // invalid password
                throw new Exception("Invalid password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDTO.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id )

            };

            var token = _jwtTokenService.GenerateToken(claims);

            return token;
        }

        public async Task<string> Register(RegisterUserDTO registerUserDTO)
        {
            if (!string.IsNullOrEmpty(registerUserDTO.Username) && !string.IsNullOrEmpty(registerUserDTO.Password))
            {

                await _userService.CreateUserAsync(registerUserDTO);
                return "User registered successfully.";

            }
            
            return null;

        }
    }
}
