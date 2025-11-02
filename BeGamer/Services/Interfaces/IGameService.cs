using BeGamer.DTOs.Game;
using BeGamer.DTOs.GameEvent;
using BeGamer.Models;
using BeGamer.Repositories;
using BeGamer.Services.Interfaces.common;

namespace BeGamer.Services.Interfaces
{
    public interface IGameService : IBaseService<GameDTO,CreateGameDTO, UpdateGameDTO>
    {

        //Task<GameDTO> CreateGameAsync(CreateGameDTO createGameDTO);
        //Task<IEnumerable<GameDTO>> GetAllGamesAsync();
        //Task<GameDTO> GetGameById(Guid id);
        //Task<GameDTO> UpdateGame(Guid id, UpdateGameDTO updateGameDTO);
        //Task<bool> DeleteGame(Guid id);
        //bool GameExistsById(Guid id);
    }
}
