using BeGamer.DTOs.Address;
using BeGamer.Models;
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
                IEnumerable<AddressDTO> addresses = await _addressService.GetAllAsync();

                _logger.LogInformation("Successfully returned {Count} addresses.", addresses.Count());
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving addresses.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //GET: api/Addresses
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddress(Guid id)
        {
            _logger.LogInformation("API request received to get Address with ID: {AddressId}", id);

            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid Address ID: {GameId} provided.", id);
                    return BadRequest($"Invalid Address with ID {id} provided.");
                }

                AddressDTO? address = await _addressService.GetByIdAsync(id);

                if (address is null)
                {
                    _logger.LogWarning("Address with ID: {AddressId} not found.", id);
                    return NotFound($"Address with ID {id} not found.");
                }

                _logger.LogInformation("API request: Address with ID: {AddressId} returned successfully.", id);
                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating Address ID: {AddressId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
