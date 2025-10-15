using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace BeGamer.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

       
        public string? Nickname { get; set; }

        public List<GameEvent> OrganizedEvents { get; set; } = new List<GameEvent>();

        public List<GameEvent> EventsToAttend { get; set; } = new List<GameEvent>();
         
    }
}
