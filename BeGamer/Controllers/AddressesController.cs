using BeGamer.Models;
using BeGamer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeGamer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly AddressService _addressService;

        public AddressesController(ILogger<AddressesController> logger, AddressService addressService)
        {
            _logger = logger;
            _addressService = addressService;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAdresses()
        {
            _logger.LogInformation("API request received to get all addresses.");

            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();

                _logger.LogInformation("Successfully returned {Count} addreses.", addresses.Count());
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving addreses.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
