using BeGamer.DTOs;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginData)
    {
        if (loginData == null || string.IsNullOrEmpty(loginData.Username) || string.IsNullOrEmpty(loginData.Password))
        {
            return BadRequest("Invalid login data.");
        }
        _authService.Login(loginData.Username, loginData.Password);

        return Unauthorized();
    }
}
