using BeGamer.DTOs.GameEvent;
using BeGamer.Models;
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

        public GameEventsController(
            IGameEventService gameEventService,
            ILogger<GameEventsController> logger)
        {
            _gameEventService = gameEventService;
            _logger = logger;
        }

        // GET: api/GameEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEventDTO>>> GetGameEvents()
        {
            _logger.LogInformation("API request received to get all GameEvents.");

            try
            {
                var gameEvents = await _gameEventService.GetAllAsync();

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

            var gameEvent = await _gameEventService.GetByIdAsync(id);

            if (gameEvent is null)
            {
                _logger.LogWarning("API request: GameEvent with ID: {GameEventId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("API request: GameEvent with ID: {GameEventId} returned successfully.", id);
            return Ok(gameEvent);
        }

        // PUT: api/GameEvents/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGameEvent(Guid id, UpdateGameEventDTO updateGameEventDTO)
        //{
        //    if (updateGameEventDTO is null)
        //    {
        //        _logger.LogWarning("UpdateGameEventDTO is null. Cannot update GameEvent.");
        //        return BadRequest("Wrong UpdateGameEventDTO data were provided.");
        //    }

        //    try
        //    {
        //        await _gameEventService.UpdateGameEvent(id, updateGameEventDTO);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating GameEvent with ID: {GameEventId}", id);
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }

        //    return NoContent();
        //}

        // POST: api/GameEvents
        [HttpPost]
        public async Task<ActionResult<GameEventDTO>> PostGameEvent(CreateGameEventDTO createGameEventDTO)
        {
            _logger.LogInformation("API request received to create a new GameEvent.");

            // Input validation 
            if (createGameEventDTO is null)
            {
                _logger.LogWarning("CreateGameEventDTO is null. Cannot create GameEvent.");
                return BadRequest("Wrong GameEvent data were provided.");
            }

            try
            {
                GameEventDTO gameEvent = await _gameEventService.CreateAsync(createGameEventDTO);

                if (gameEvent is null)
                {
                    _logger.LogWarning("GameEvent creation failed. Service returned null.");
                    return StatusCode(500, "Failed to create GameEvent.");
                }

                _logger.LogInformation("GameEvent with ID {GameEventId} was successfully created.", gameEvent.Id);
                return CreatedAtAction(nameof(GetGameEventById), new { id = gameEvent.Id }, gameEvent);
            }catch (ArgumentException ix)
            {
                _logger.LogError(ix, "Invalid data provided for creating GameEvent.");
                return BadRequest(ix.Message);

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
                var success = await _gameEventService.DeleteAsync(id);

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

        // GET: api/GameEvents/nearby?lat=50.0&lon=14.0&distance=1000
        //[HttpGet("nearby")]
        //public async Task<ActionResult<List<GameEvent>>> GetNearby(
        //    [FromQuery] double lat,
        //    [FromQuery] double lon,
        //    [FromQuery] double distance)
        //{
        //    _logger.LogInformation(
        //        "API request received to get nearby GameEvents. Latitude: {Latitude}, Longitude: {Longitude}, Distance: {Distance}m",
        //        lat, lon, distance);

        //    try
        //    {
        //        var nearbyEvents = await _gameEventService.GetNearbyGameEvents(lat, lon, distance);

        //        if (nearbyEvents == null || !nearbyEvents.Any())
        //        {
        //            _logger.LogWarning(
        //                "No GameEvents found near location ({Latitude}, {Longitude}) within {Distance} meters.",
        //                lat, lon, distance);

        //            return NotFound("No nearby GameEvents found.");
        //        }

        //        _logger.LogInformation(
        //            "{Count} GameEvents found near location ({Latitude}, {Longitude}) within {Distance} meters.",
        //            nearbyEvents.Count(), lat, lon, distance);

        //        return Ok(nearbyEvents);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(
        //            ex,
        //            "An error occurred while attempting to retrieve nearby GameEvents at ({Latitude}, {Longitude}) within {Distance} meters.",
        //            lat, lon, distance);

        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}
    }
}
