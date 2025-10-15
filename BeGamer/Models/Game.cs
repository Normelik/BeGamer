using BeGamer.Enums;
using System.ComponentModel.DataAnnotations;

namespace BeGamer.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int MinPlayers { get; set; } = 1;

        public int MaxPlayers { get; set; }

        public BoardGameType Type { get; set; }
        public List<GameEvent>? Events { get; set; } = new List<GameEvent>();

    }
}
