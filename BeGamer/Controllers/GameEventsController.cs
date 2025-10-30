using BeGamer.DTOs.GameEvent;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeGamer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameEventsController : ControllerBase
    {
        private readonly IGameEventService _gameEventService;
        private readonly ILogger<GameEventsController> _logger;

        public GameEventsController(IGameEventService GameEventService, ILogger<GameEventsController> logger)
        {
            _gameEventService = GameEventService;
            _logger = logger;
        }

        // GET: api/GameEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEventDTO>>> GetGameEvents()
        {
            _logger.LogInformation("API request received to get all GameEvents.");

            try
            {
                var gameEvents = await _gameEventService.GetAllGameEventsAsync();

                _logger.LogInformation("Successfully returned {Count} GameEvents.", gameEvents.Count());
                return Ok(gameEvents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving GameEvents.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/GameEvents/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameEventDTO>> GetGameEventById(Guid id)
        {
            _logger.LogInformation("API request received to get GameEvent with ID: {GameEventId}", id);

            var gameEvent = await _gameEventService.GetGameEventById(id);

            if (gameEvent == null)
            {
                _logger.LogWarning("API request: GameEvent with ID: {GameEventId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("API request: GameEvent with ID: {GameEventId} returned successfully.", id);
            return Ok(gameEvent);
        }

        // PUT: api/GameEvents/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameEvent(GameEventDTO gameEventDTO)
        {
            if(gameEventDTO == null)
            {
                _logger.LogWarning("GameEventDTO is null. Cannot update GameEvent.");
                return BadRequest("Wrong GameEvent data were provided.");
            }

            try
            {
                await _gameEventService.UpdateGameEvent(gameEventDTO.Id, gameEventDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating GameEvent with ID: {GameEventId}", gameEventDTO.Id);
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return NoContent();
        }

        // POST: api/GameEvents
        [HttpPost]
        public async Task<ActionResult<GameEventDTO>> PostGameEvent(CreateGameEventDTO createGameEventDTO)
        {
            _logger.LogInformation("API request received to create a new GameEvent.");

            // Input validation 
            if (createGameEventDTO == null)
            {
                _logger.LogWarning("CreateGameEventDTO is null. Cannot create GameEvent.");
                return BadRequest("Wrong GameEvent data were provided.");
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var GameEvent = await _gameEventService.CreateGameEvent(Guid.Parse(userId!), createGameEventDTO);

                if (GameEvent == null)
                {
                    _logger.LogWarning("GameEvent creation failed. Service returned null.");
                    return StatusCode(500, "Failed to create GameEvent.");
                }

                _logger.LogInformation("GameEvent with ID {GameEventId} was successfully created.", GameEvent.Id);
                return CreatedAtAction(nameof(GetGameEventById), new { id = GameEvent.Id }, GameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new GameEvent.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE: api/GameEvents/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameEvent(Guid id)
        {
            _logger.LogInformation("API request received to delete GameEvent with ID: {GameEventId}", id);

            try
            {
                var success = await _gameEventService.DeleteGameEvent(id);

                if (success)
                {
                    _logger.LogInformation("GameEvent with ID: {GameEventId} was successfully deleted.", id);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("GameEvent with ID: {GameEventId} not found. Delete operation aborted.", id);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete GameEvent with ID: {GameEventId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
