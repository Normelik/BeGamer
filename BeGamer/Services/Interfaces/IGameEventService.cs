

using BeGamer.DTOs;

namespace BeGamer.Services.Interfaces
{
    public interface IGameEventService
    {
        IEnumerable<GameEventDto> getAllEvents();
    }
}
