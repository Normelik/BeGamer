using BeGamer.Enums;

namespace BeGamer.DTOs.Address
{
    public record AddressDTO
        
    {

        public Guid Id { get; init; }
        public required string Street { get; init; }
        public required string City { get; init; }
        public required string PostalCode { get; init; }
        public required string Country { get; init; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }

    }
}
