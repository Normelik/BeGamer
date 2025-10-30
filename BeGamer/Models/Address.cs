namespace BeGamer.Models
{
    public class Address
    {
        public Guid Id { get; set; }

        public required string Street { get; set; }

        public required string City { get; set; }

        public required string PostalCode { get; set; }

        public required string Country { get; set; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }

    }
}
