using BeGamer.DTOs;
using BeGamer.Models;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Mappers
{
    public class GameEventMapper
    {
        public static GameEventDto ToDto(GameEvent gameEvent)
        {
            if (gameEvent == null) return null;

            return new GameEventDto
            {
                Id = gameEvent.Id,
                Title = gameEvent.Title,
                LocationId = gameEvent.LocationId,
                DateEvent = gameEvent.DateEvent,
                OrganizerId = gameEvent.OrganizerId,
                MaxPlayers = gameEvent.MaxPlayers,
                MinPlayers = gameEvent.MinPlayers,
                RegistrationDeadline = gameEvent.RegistrationDeadline,
                Note = gameEvent.note,
                GameId = gameEvent.GameId,
                // Pokud chceš, můžeš přidat např. Organizer.Name nebo Location.Address apod.
            };
        }
        public static IEnumerable<GameEventDto> ToDtoList(IEnumerable<GameEvent> gameEvents)
        {
            if (gameEvents == null) return Enumerable.Empty<GameEventDto>();

            return gameEvents.Select(e => ToDto(e));
        }

        public static GameEvent toEntity(CreateGameEventDto dto)
        {
            if (dto == null) return null;
            return new GameEvent
            {
                Id = new Guid(),
                Title = dto.Title,
                Location = dto.Location,
                DateEvent = dto.DateEvent,
            };
        }
    }
}
