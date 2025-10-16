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

            return new GameEventDto(
                gameEvent.Id,
                gameEvent.Title,
                gameEvent.Location,
                gameEvent.DateEvent,
                gameEvent.OrganizerId,
                gameEvent.MaxPlayers,
                gameEvent.MinPlayers,
                gameEvent.RegistrationDeadline,
                gameEvent.Note,
                gameEvent.GameId
            );
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
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Location = dto.Location,
                DateEvent = dto.DateEvent,
                Organizer = null, // To be set in the service layer
                OrganizerId = Guid.Empty, // To be set in the service layer
                MaxPlayers = dto.MaxPlayers,
                MinPlayers = dto.MinPlayers,
                RegistrationDeadline = dto.RegistrationDeadline,
                Note = dto.Note,
                GameId = dto.GameId,
                Game = null // To be set in the service layer or elsewhere
            };
        }
    }
}
