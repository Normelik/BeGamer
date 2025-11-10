using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;

namespace BeGamer.Repositories.Impl
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(AppDbContext context, ILogger<Game> logger)
            : base(context, logger, context.Game)
        {
        }
    }
}
