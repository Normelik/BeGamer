using BeGamer.DTOs.User;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("API request received to create a new user.");

            // Input validation (basic, controller-level)
            if (createUserDTO == null)
            {
                _logger.LogWarning("CreateUserDTO is null. Cannot create user.");
                return BadRequest("Wrong user data were provided.");
            }

            if (string.IsNullOrWhiteSpace(createUserDTO.Username))
            {
                _logger.LogWarning("CreateUserDTO has missing required fields: Username");
                return BadRequest("Username is required.");
            }

            try
            {
                var createdUser = await _userService.CreateUserAsync(createUserDTO);

                if (createdUser == null)
                {
                    _logger.LogWarning("User creation failed. Service returned null.");
                    return StatusCode(500, "Failed to create user.");
                }

                _logger.LogInformation("User with ID {UserId} was successfully created.", createdUser.Id);
                return CreatedAtAction(nameof(GetUser), new {id = createdUser.Id}, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            _logger.LogInformation("API request received to get all users.");

            try
            {
                var users = await _userService.GetAllUsers();

                _logger.LogInformation("Successfully returned {Count} users.", users.Count());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            _logger.LogInformation("API request received to get user with ID: {UserId}", id);

            var currentUser = await _userService.GetUserById(id);

            if (currentUser == null)
            {
                _logger.LogWarning("API request: User with ID: {UserId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("API request: User with ID: {UserId} returned successfully.", id);
            return Ok(currentUser);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UpdateUserDTO updateUserDTO)
        {
            _logger.LogInformation("API request received to update user with ID: {UserId}", id);

            if (updateUserDTO == null)
            {
                _logger.LogWarning("Update request for user ID {UserId} contained null payload.", id);
                return BadRequest(new { message = "No user data was provided for update." });
            }

            // Check if user exists
            if (!await _userService.UserExistsById(id))
            {
                _logger.LogWarning("User with ID {UserId} not found. Update aborted.", id);
                return NotFound(new { message = $"User with ID {id} was not found for update." });
            }

            try
            {
                _logger.LogInformation("Updating user with ID {UserId}...", id);

                await _userService.UpdateUser(id, updateUserDTO);

                _logger.LogInformation("User with ID {UserId} updated successfully.", id);
                return NoContent(); // 204 No Content = úspěšná aktualizace
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating user with ID {UserId}", id);
                return StatusCode(500, new
                {
                    message = $"A concurrency error occurred while updating the user with ID {id}.",
                    details = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating user with ID {UserId}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            _logger.LogInformation("API request received to delete user with ID: {UserId}", id);

            try
            {
                var success = await _userService.DeleteUser(id);

                if (success)
                {
                    _logger.LogInformation("User with ID: {UserId} was successfully deleted.", id);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("User with ID: {UserId} not found. Delete operation aborted.", id);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete user with ID: {UserId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
