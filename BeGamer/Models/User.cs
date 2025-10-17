namespace BeGamer.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }
               
        public string? Nickname { get; set; }

        public List<GameEvent> OrganizedEvents { get; set; } = [];

        public List<GameEvent> EventsToAttend { get; set; } = [];
         
    }
}
