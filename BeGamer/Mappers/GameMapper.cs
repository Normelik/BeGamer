using BeGamer.DTOs.Game;
using BeGamer.Enums;
using BeGamer.Models;

namespace BeGamer.Mappers
{
    public class GameMapper
    {
        public GameDTO ToDTO(Game game)
        {
            if (game == null) return null;

            return new GameDTO(
                game.Id,
                game.Title,
                game.MaxPlayers,
                game.MinPlayers,
                game.Type
            );
        }
        public IEnumerable<GameDTO> ToDtoList(IEnumerable<Game> games)
        {
            if (games == null) return Enumerable.Empty<GameDTO>();

            return games.Select(e => ToDTO(e));
        }

        public Game ToEntity(CreateGameDTO dto)
        {
            return new Game
            {
                Title = dto.Title,
                MinPlayers = dto.MinPlayers,
                MaxPlayers = dto.MaxPlayers,
                Type = (Enums.BoardGameType)dto.Type
            };
        }
        public void ToExistingEntity(UpdateGameDTO dto, Game entity)
        {
            if (dto.Title != entity.Title)
            {
                entity.Title = dto.Title;
            }

            if(dto.MinPlayers != entity.MinPlayers)
            {
                entity.MinPlayers = dto.MinPlayers;
            }

            if(dto.MaxPlayers != entity.MaxPlayers)
            {
                entity.MaxPlayers = dto.MaxPlayers;
            }

            BoardGameType type = (BoardGameType)dto.Type;
            if(type != entity.Type) {
                entity.Type = type;
            }
        }
    }
}
