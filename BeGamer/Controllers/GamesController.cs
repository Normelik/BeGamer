using BeGamer.DTOs.Game;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeGamer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _GameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService GameService, ILogger<GamesController> logger)
        {
            _GameService = GameService;
            _logger = logger;
        }

        // POST: api/Games
        [HttpPost]
        //public async Task<ActionResult<GameDTO?>> PostGame(CreateGameDTO createGameDTO)
        //{
        //    _logger.LogInformation("API request received to create a new Game.");

        //    // Input validation (basic, controller-level)
        //    if (createGameDTO is null)
        //    {
        //        _logger.LogWarning("CreateGameDTO is null. Cannot create Game.");
        //        return BadRequest("Wrong Game data were provided.");
        //    }

        //    try
        //    {
        //        var createdGame = await _GameService.CreateAsync(createGameDTO);

        //        if (createdGame == null)
        //        {
        //            _logger.LogWarning("Game creation failed. Service returned null.");
        //            return StatusCode(500, "Failed to create Game.");
        //        }

        //        _logger.LogInformation("Game with ID {GameId} was successfully created.", createdGame.GameId);
        //        return Ok(createdGame);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while creating a new Game.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGames()
        {
            _logger.LogInformation("API request received to get all Games.");

            try
            {
                var Games = await _GameService.GetAllAsync();

                _logger.LogInformation("Successfully returned {Count} Games.", Games.Count());
                return Ok(Games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Games.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/Games/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> GetGame(Guid id)
        {
            _logger.LogInformation("API request received to get Game with ID: {GameId}", id);

            var currentGame = await _GameService.GetByIdAsync(id);

            if (currentGame == null)
            {
                _logger.LogWarning("API request: Game with ID: {GameId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("API request: Game with ID: {GameId} returned successfully.", id);
            return Ok(currentGame);
        }

        // PUT: api/Games/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGame(Guid id, UpdateGameDTO updateGameDTO)
        //{
        //    _logger.LogInformation("API request received to update Game with ID: {GameId}", id);

        //    if (updateGameDTO == null)
        //    {
        //        _logger.LogWarning("Update request for Game ID {GameId} contained null payload.", id);
        //        return BadRequest(new { message = "No Game data was provided for update." });
        //    }

        //    // Check if Game exists
        //    if (_GameService.ExistsById(id).Result == false)
        //    {
        //        _logger.LogWarning("Game with ID {GameId} not found. Update aborted.", id);
        //        return NotFound(new { message = $"Game with ID {id} was not found for update." });
        //    }

        //    try
        //    {
        //        _logger.LogInformation("Updating Game with ID {GameId}...", id);

        //        await _GameService.UpdateAsync(id, updateGameDTO);

        //        _logger.LogInformation("Game with ID {GameId} updated successfully.", id);
        //        return NoContent();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        _logger.LogError(ex, "Concurrency error while updating Game with ID {GameId}", id);
        //        return StatusCode(500, new
        //        {
        //            message = $"A concurrency error occurred while updating the Game with ID {id}.",
        //            details = ex.Message
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unexpected error occurred while updating Game with ID {GameId}", id);
        //        return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        //    }
        //}

        // DELETE: api/Games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            _logger.LogInformation("API request received to delete Game with ID: {GameId}", id);

            try
            {
                var success = await _GameService.DeleteAsync(id);

                if (success)
                {
                    _logger.LogInformation("Game with ID: {GameId} was successfully deleted.", id);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("Game with ID: {GameId} not found. Delete operation aborted.", id);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete Game with ID: {GameId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
