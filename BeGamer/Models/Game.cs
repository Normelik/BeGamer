using BeGamer.Enums;

namespace BeGamer.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public int MinPlayers { get; set; } = 1;

        public int MaxPlayers { get; set; }

        public BoardGameType Type { get; set; }
        public List<GameEvent>? Events { get; set; } = [];

    }
}
