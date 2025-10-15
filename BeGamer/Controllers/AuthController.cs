using BeGamer.DTOs;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyApp.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginData)
    {
        // Tady bys měl mít reálnou validaci uživatele
        if (loginData.Username == "admin" && loginData.Password == "password")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, loginData.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = _jwtTokenService.GenerateToken(claims);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
}
