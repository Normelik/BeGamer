using AutoMapper;
using BeGamer.DTOs.Game;
using BeGamer.Models;
using BeGamer.Repositories.common;
using BeGamer.Repositories.Interfaces;
using BeGamer.Services.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;

namespace BeGamer.Services
{
    public class GameService : BaseAppService<Game, GameDTO, CreateGameDTO, UpdateGameDTO>, IGameService
    {

        public GameService(
            GuidGenerator guidGenerator,
            IMapper mapper,
            IGameRepository repository,
            ILogger<GameService> logger
        ) : base(guidGenerator, mapper,repository, logger)
        {
        }



        public override async Task<GameDTO> CreateAsync(CreateGameDTO createDto)
        {
            _logger.LogInformation("Start creating new Game.");

            try
            {
                if (createDto == null)
                {
                    _logger.LogWarning("CreateGameDTO is null. Aborting creation.");
                    throw new ArgumentNullException(nameof(createDto), "CreateGameDTO cannot be null");
                }

                Game game = _mapper.Map<CreateGameDTO, Game>(createDto);

                _logger.LogInformation("Game with ID: {GameId} successfully created.", game.ToString());
                // Assign a unique GUID using the GuidGenerator utility
                game.Id = await _guidGenerator.GenerateUniqueGuidAsync(ExistsById);

                Game createdGame = await _genericRepository.CreateAsync(game);

                _logger.LogInformation("Game with ID: {GameId} successfully created.", game.Id);

                return _mapper.Map<Game, GameDTO>(createdGame);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new Game.");
                throw;
            }
        }

        public override async Task<GameDTO> UpdateAsync(Guid id, UpdateGameDTO updateDto)
        {
            _logger.LogInformation("Attempting to update Game with ID: {GameId}", id);

            try
            {
                var game = await _genericRepository.FindByIdAsync(id);

                if (game == null)
                {
                    _logger.LogWarning("Game with ID: {GameId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Game with ID: {GameId} found. Updating fields...", id);

                // Buď ručně:
                Game? updatedGame = _mapper.Map<UpdateGameDTO, Game>(updateDto);

                await _genericRepository.SaveChangesAsync();

                _logger.LogInformation("Game with ID: {GameId} successfully updated.", id);

                return _mapper.Map<Game, GameDTO>(updatedGame);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Game with ID: {GameId}", id);
                throw;
            }
        }
    }
}
