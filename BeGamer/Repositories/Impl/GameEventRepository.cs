using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Repositories.Impl
{
    public class GameEventRepository : GenericRepository<GameEvent>, IGameEventRepository
    {
        public GameEventRepository(AppDbContext context, ILogger<GameEvent> logger)
            : base(context, logger, context.GameEvents)
        {
        }

        public async Task<GameEvent?> GetGameEventByIdWithParticipantsAsync(Guid id)
        {
            return await _context.GameEvents
                                .Include(e => e.Participants)
                                .FirstOrDefaultAsync(ge => ge.Id == id);
        }
    }
}
