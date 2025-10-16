using Microsoft.AspNetCore.Mvc;
using BeGamer.Services;
using BeGamer.DTOs;
using Microsoft.AspNetCore.Authorization;
using BeGamer.Services.Interfaces;

namespace BeGamer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class GameEventsController : ControllerBase
    {
        private readonly IGameEventService GameEventService;

        public GameEventsController(IGameEventService GameEventService)
        {
            this.GameEventService = GameEventService;
        }

        // GET: api/GameEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEventDto>>> GetGameEvents()
        {
          
            return Ok(await GameEventService.GetAllEvents());
        }

        // GET: api/GameEvents/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<GameEvent>> GetGameEvent(Guid id)
        //{
        //    var gameEvent = await _context.GameEvents.FindAsync(id);

        //    if (gameEvent == null)
        //    {
        //        return NotFound();
        //    }

        //    return gameEvent;
        //}

        //// PUT: api/GameEvents/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGameEvent(Guid id, GameEvent gameEvent)
        //{
        //    if (id != gameEvent.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(gameEvent).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GameEventExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/GameEvents
        [HttpPost]
        public async Task<ActionResult<GameEventDto>> PostGameEvent(CreateGameEventDto gameEvent)
        {
      

            return Created(nameof(GetGameEvents), await GameEventService.CreateEvent(gameEvent));
        }

        //// DELETE: api/GameEvents/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGameEvent(int id)
        //{
        //    var gameEvent = await _context.GameEvents.FindAsync(id);
        //    if (gameEvent == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.GameEvents.Remove(gameEvent);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool GameEventExists(Guid id)
        //{
        //    return _context.GameEvents.Any(x => x.Id == id);
        //}
    }
}
