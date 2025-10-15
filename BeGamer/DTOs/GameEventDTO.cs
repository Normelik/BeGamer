using BeGamer.Models;

namespace BeGamer.DTOs
{
    public record GameEventDto(
      Guid Id,
      string Title,
      Address LocationId, // Address Id
      DateTime DateEvent,
      Guid OrganizerID, // User Id
      int MaxPlayers,
      int MinPlayers,
      DateOnly RegistrationDeadline,
      string Note,
      Guid GameId
  );
}
