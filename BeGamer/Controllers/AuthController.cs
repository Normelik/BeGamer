using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Services;
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
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return BadRequest("Invalid login data.");
        }
        
        var token =  await _authService.Login(loginDTO);
        return Ok(token);
        ;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
    {
        if (registerUserDTO == null || string.IsNullOrEmpty(registerUserDTO.Username) || string.IsNullOrEmpty(registerUserDTO.Password))
        {
            return BadRequest("Invalid registration data.");
        }
        await _authService.Register(registerUserDTO);
        return Ok();
    }

}
