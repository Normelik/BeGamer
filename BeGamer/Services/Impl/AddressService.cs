using AutoMapper;
using BeGamer.DTOs.Address;
using BeGamer.Models;
using BeGamer.Repositories.Interfaces;
using BeGamer.Services.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;

namespace BeGamer.Services
{

    public class AddressService : BaseAppService<Address, AddressDTO, CreateAddressDTO, UpdateAddressDTO>, IAddressService
    {
        public AddressService(
            GuidGenerator guidGenerator,
            IMapper mapper,
            IAddressRepository repository,
            ILogger<AddressService> logger
        ) : base(guidGenerator, mapper, repository, logger)
        {
        }
        public override Task<AddressDTO> CreateAsync(CreateAddressDTO createDto)
        {
            throw new NotImplementedException();
        }

        public override Task<AddressDTO> UpdateAsync(Guid id, UpdateAddressDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
