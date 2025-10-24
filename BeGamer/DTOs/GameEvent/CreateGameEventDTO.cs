using BeGamer.Models;

namespace BeGamer.DTOs.GameEvent
{
    public record CreateGameEventDTO(
    string Title,
    Guid LocationId,
    DateTime DateEvent,
    int MaxPlayers,
    int MinPlayers,
    DateOnly RegistrationDeadline,
    string Note,
    Guid GameId
);
}


