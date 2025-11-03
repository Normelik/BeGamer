using BeGamer.DTOs.Game;
using BeGamer.Models;
using BeGamer.Services.Interfaces.common;

namespace BeGamer.Services.Interfaces
{
    public interface IGameService : IBaseAppService<Game, GameDTO, CreateGameDTO, UpdateGameDTO>
    {
    }
}
