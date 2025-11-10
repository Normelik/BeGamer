using BeGamer.DTOs.Game;
using BeGamer.Models;
using BeGamer.Services.common;

namespace BeGamer.Services.Interfaces
{
    public interface IGameService : IBaseAppService<Game, GameDTO, CreateGameDTO, UpdateGameDTO>
    {
    }
}
