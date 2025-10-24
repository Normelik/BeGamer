using BeGamer.Data;
using BeGamer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BeGamer.Services
{
    
    public class AddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly AppDbContext _context;

        public AddressService(ILogger<AddressService> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //GET ALL USERS
        //public  Task<List<Address>> GetAllAddressesAsync()
        //{
        //    _logger.LogInformation("Fetching all addresses from the database.");

        //    try
        //    {
        //        var addresses =  _context.Addresses.ToList();
        //        _logger.LogInformation("Fetched {Count} adresses from the database.", addresses.Count);

        //        return Task.FromResult(addresses);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching adresses.");
        //        throw;
        //    }
        //}
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
