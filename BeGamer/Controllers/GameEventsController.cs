using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeGamer.Models;
using BeGamer.Services;

namespace BeGamer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameEventsController : ControllerBase
    {
        private GameEventService gameEventService;

        public GameEventsController(GameEventService gameEventService)
        {
            this.gameEventService = gameEventService;
        }

        // GET: api/GameEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEvent>>> GetGameEvents()
        {
            return Ok(gameEventService.getAllEvents());
        }

        // GET: api/GameEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameEvent>> GetGameEvent(Guid id)
        {
            var gameEvent = await _context.GameEvents.FindAsync(id);

            if (gameEvent == null)
            {
                return NotFound();
            }

            return gameEvent;
        }

        // PUT: api/GameEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameEvent(Guid id, GameEvent gameEvent)
        {
            if (id != gameEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameEvent>> PostGameEvent(GameEvent gameEvent)
        {
            _context.GameEvents.Add(gameEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameEvent", new { id = gameEvent.Id }, gameEvent);
        }

        // DELETE: api/GameEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameEvent(int id)
        {
            var gameEvent = await _context.GameEvents.FindAsync(id);
            if (gameEvent == null)
            {
                return NotFound();
            }

            _context.GameEvents.Remove(gameEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameEventExists(Guid id)
        {
            return _context.GameEvents.Any(x => x.Id == id);
        }
    }
}
