using BeGamer.DTOs.GameEvent;
using BeGamer.Models;
using BeGamer.Services.common;

namespace BeGamer.Services.Interfaces
{
    public interface IGameEventService : IBaseAppService<GameEvent,GameEventDTO, CreateGameEventDTO, UpdateGameEventDTO>
    {
        //Task<GameEventDTO> CreateGameEvent(Guid id, CreateGameEventDTO createGameEventDTO);
        //Task<GameEventDTO> UpdateGameEvent(Guid id, UpdateGameEventDTO updateGameEventDTO);
        //Task<IEnumerable<GameEvent>> GetNearbyGameEvents(double userLatitude,
        //                                    double userLongitude,
        //                                    double distanceInMeters);
    }
}
