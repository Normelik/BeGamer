using BeGamer.DTOs;
using BeGamer.DTOs.Auth;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace MyApp.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
    {
        _logger.LogInformation("Login attempt for username: {Username}", loginDTO.Username);

        try
        {
            var token = await _authService.LoginAsync(loginDTO);

            if (token == null)
            {
                _logger.LogWarning("Invalid login attempt for username: {Username}", loginDTO.Username);
                return Unauthorized("Invalid username or password.");
            }

            _logger.LogInformation("User {Username} logged in successfully.", loginDTO.Username);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while logging in user: {Username}", loginDTO.Username);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        _logger.LogInformation("Register attempt for username: {Username}", registerUserDTO.Username);

        try
        {
            var result = await _authService.RegisterAsync(registerUserDTO);
            return StatusCode(200, result.ToString());
        }
        catch (InvalidOperationException ex)
        {
            // Username already exists.
            _logger.LogWarning(ex, "Validation error during registration for {Username}", registerUserDTO.Username);
            return BadRequest(new { message = ex.Message });
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Database error during registration for {Username}", registerUserDTO.Username);
            return StatusCode(500, new { message = "Database error occurred." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during registration for {Username}", registerUserDTO.Username);
            return StatusCode(500, new { message = "Unexpected server error." });
        }
    }
}
