
using Microsoft.AspNetCore.Identity;

namespace BeGamer.Models
{
    public class CustomUser : IdentityUser<Guid>
    {
        public string? Nickname { get; set; }

        public List<GameEvent> OrganizedEvents { get; set; } = [];

        public List<GameEvent> EventsToAttend { get; set; } = [];
         
    }
}
