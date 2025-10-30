namespace BeGamer.Models
{
    public class GameEvent
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required Guid LocationId { get; set; }
        public required Address Location { get; set; }
        public DateTime DateEvent { get; set; }
        public required Guid OrganizerId { get; set; }
        public CustomUser? Organizer { get; set; }
        public List<CustomUser> Participants { get; set; } = [];
        public int MaxPlayers { get; set; }
        public int MinPlayers { get; set; } = 0;
        public DateOnly RegistrationDeadline { get; set; }
        public string? Note { get; set; }
        public required Guid GameId { get; set; }
        public required Game Game { get; set; }
    }
}
