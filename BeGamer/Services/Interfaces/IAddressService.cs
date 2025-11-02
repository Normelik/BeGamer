using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
    }
}
