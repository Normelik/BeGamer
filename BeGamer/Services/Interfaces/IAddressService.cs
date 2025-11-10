using BeGamer.DTOs.Address;
using BeGamer.Models;
using BeGamer.Services.common;

namespace BeGamer.Services.Interfaces
{
    public interface IAddressService : IBaseAppService<Address,AddressDTO, CreateAddressDTO, UpdateAddressDTO>
    {
    }
}
