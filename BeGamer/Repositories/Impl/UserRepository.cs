using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;

namespace BeGamer.Repositories.Impl
{
    public class UserRepository : GenericRepository<CustomUser>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger<CustomUser> logger)
            : base(context, logger, context.Users)
        {
        }
    }
}
