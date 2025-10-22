using BeGamer.Models;

namespace BeGamer.DTOs.GameEvent
{
    public record GameEventDTO(
      Guid Id,
      string Title,
      Address Location,
      DateTime DateEvent,
      string OrganizerID, // CustomUser Id
      int MaxPlayers,
      int MinPlayers,
      DateOnly RegistrationDeadline,
      string Note,
      Guid GameId
  );
}
