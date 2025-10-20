using BeGamer.DTOs;
using BeGamer.DTOs.User;
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
    public IActionResult Login([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return BadRequest("Invalid login data.");
        }
        _authService.Login(loginDTO);

        return Unauthorized();
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserDTO registerUserDTO)
    {
        if (registerUserDTO == null || string.IsNullOrEmpty(registerUserDTO.Username) || string.IsNullOrEmpty(registerUserDTO.Password))
        {
            return BadRequest("Invalid registration data.");
        }
        _authService.Register(registerUserDTO);
        return Unauthorized();
    }
}
