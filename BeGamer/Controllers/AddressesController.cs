using BeGamer.Models;
using BeGamer.Services;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeGamer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IAddressService _addressService;

        public AddressesController(ILogger<AddressesController> logger, IAddressService addressService)
        {
            _logger = logger;
            _addressService = addressService;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
        {
            _logger.LogInformation("API request received to get all addresses.");

            try
            {
                var addresses = await _addressService.GetAllAsync();

                _logger.LogInformation("Successfully returned {Count} addresses.", addresses.Count());
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving addresses.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
