

using BeGamer.DTOs;

namespace BeGamer.Services.Interfaces
{
    public interface IGameEventService
    {
        Task<IEnumerable<GameEventDto>> GetAllEvents();
        Task<GameEventDto> GetEventById(Guid id);
        Task<GameEventDto> CreateEvent(CreateGameEventDto createGameEventDto);
        Task<bool> UpdateEvent(Guid id, GameEventDto gameEventDto);
        Task<bool> DeleteEvent(Guid id);
    }
}
