using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;

namespace BeGamer.Repositories.Impl
{
    public class GameEventRepository : GenericRepository<GameEvent>, IGameEventRepository
    {
        public GameEventRepository(AppDbContext context, ILogger<GameEvent> logger)
            : base(context, logger, context.GameEvents)
        {
        }
    }
}
