using BeGamer.Enums;

namespace BeGamer.DTOs.Game
{
    public record GameDTO(
        Guid GameId,
        string Title,
        int MinPlayers,
        int MaxPlayers,
        BoardGameType Type)
    {};
}
