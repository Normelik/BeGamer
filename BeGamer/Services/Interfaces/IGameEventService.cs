using BeGamer.DTOs.GameEvent;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IGameEventService
    {
        Task<IEnumerable<GameEventDTO>> GetAllGameEventsAsync();
        Task<GameEventDTO> GetGameEventById(Guid id);
        Task<GameEventDTO> CreateGameEvent(Guid id, CreateGameEventDTO createGameEventDTO);
        Task<GameEventDTO> UpdateGameEvent(Guid id, GameEventDTO gameEventDto);
        Task<bool> DeleteGameEvent(Guid id);

        Task<List<GameEvent>> GetNearbyGameEvents(double userLatitude,
                                            double userLongitude,
                                            double distanceInMeters);
    }
}
