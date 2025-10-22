using Microsoft.AspNetCore.Identity;

namespace BeGamer.Models
{
    public class CustomUser : IdentityUser
    {
        public string? Nickname { get; set; }

        public List<GameEvent> OrganizedEvents { get; set; } = [];

        public List<GameEvent> EventsToAttend { get; set; } = [];
         
    }
}
