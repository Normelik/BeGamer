using BeGamer.Models;

namespace BeGamer.DTOs.GameEvent
{
    public record CreateGameEventDTO(
    string Title,
    Address Location,
    DateTime DateEvent,
    int MaxPlayers,
    int MinPlayers,
    DateOnly RegistrationDeadline,
    string Note,
    Guid GameId
);
}


