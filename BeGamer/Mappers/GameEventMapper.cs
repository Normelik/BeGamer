using BeGamer.DTOs.GameEvent;
using BeGamer.Models;

namespace BeGamer.Mappers
{
    public class GameEventMapper
    {
        public GameEventDTO ToDTO(GameEvent gameEvent)
        {
            if (gameEvent == null) return null;

            return new GameEventDTO(
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
        public IEnumerable<GameEventDTO> ToDtoList(IEnumerable<GameEvent> gameEvents)
        {
            if (gameEvents == null) return Enumerable.Empty<GameEventDTO>();

            return gameEvents.Select(e => ToDTO(e));
        }

        public GameEvent ToEntity(CreateGameEventDTO dto)
        {
            if (dto == null) return null;
            return new GameEvent
            {
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
