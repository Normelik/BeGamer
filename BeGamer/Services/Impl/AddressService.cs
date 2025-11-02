using BeGamer.Data;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{

    public class AddressService : IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly AppDbContext _context;

        public AddressService(ILogger<AddressService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET ALL Addresses
        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {

            _logger.LogInformation("Fetching all Addresses from the database.");

            try
            {
                var addresses = await _context.Addresses.ToListAsync();
                _logger.LogInformation("Fetched {Count} Addreses from the database.", addresses.Count);

                if (addresses == null || !addresses.Any())
                {
                    return Enumerable.Empty<Address>();
                }

                return addresses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching addresses.");
                throw;
            }
        }
    }
}
