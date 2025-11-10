using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;

namespace BeGamer.Repositories.Impl
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context, ILogger<Address> logger)
            : base(context, logger, context.Addresses)
        {
        }
    }
}
