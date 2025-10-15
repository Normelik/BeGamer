using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace BeGamer.Models
{
    public class GameEvent
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public Address Location { get; set; }

        public Guid LocationId { get; set; }

        [Required]
        public DateTime DateEvent { get; set; }

        public User Organizer { get; set; }
        public Guid OrganizerId { get; set; }

        public List<User> Participants { get; set; } = new List<User>();


        public int MaxPlayers { get; set; }
        public int MinPlayers { get; set; } = 0;

        public DateOnly RegistrationDeadline { get; set; }

        public string note { get; set; }

        [Required]
        public Game Game { get; set; }
        public Guid GameId { get; set; }
    }
}
