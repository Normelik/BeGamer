namespace BeGamer.DTOs.Address
{
    public record AddressDTO(
        Guid AddressId,
        string Street,
        string City,
        string State,
        string ZipCode,
        string Country)
    {
    }
}
