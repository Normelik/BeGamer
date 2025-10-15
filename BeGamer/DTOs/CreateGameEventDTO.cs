using BeGamer.Models;

namespace BeGamer.DTOs
{
    public record CreateGameEventDto(
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


