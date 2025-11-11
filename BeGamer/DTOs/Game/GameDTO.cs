using BeGamer.Enums;

namespace BeGamer.DTOs.Game
{
    public record GameDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int MinPlayers { get; init; }
        public int MaxPlayers { get; init; }
        public BoardGameType Type { get; init; }
    }
}
