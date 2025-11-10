using BeGamer.DTOs.Address;

namespace BeGamer.DTOs.GameEvent
{
    public record GameEventDTO(
      Guid Id,
      string Title,
      AddressDTO Location,
      DateTime DateEvent,
      Guid OrganizerID, // CustomUser Id
      int MaxPlayers,
      int MinPlayers,
      DateOnly RegistrationDeadline,
      string Note,
      Guid GameId
  );
}
