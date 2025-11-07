using BeGamer.Data;
using BeGamer.DTOs.Game;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Repositories;
using BeGamer.Repositories.common;
using BeGamer.Services.Impl.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class GameService : BaseAppService<Game, GameDTO, CreateGameDTO, UpdateGameDTO, IGameRepository>, IGameService
    {
        private readonly GameMapper _gameMapper;
        private readonly GenericRepository<Game> _gameRepository;


        public GameService(
            AppDbContext context,
            ILogger<GameService> logger,
            GuidGenerator guidGenerator,
            GameMapper gameMapper,
            GenericRepository<Game> genericRepository)
            : base(context, logger, guidGenerator)
        {
            _gameMapper = gameMapper;
            _gameRepository = genericRepository;
        }

        public override async Task<GameDTO> CreateAsync(CreateGameDTO createDto)
        {
            _logger.LogInformation("Start creating new Game.");

            try
            {
                if (createDto == null) {
                    _logger.LogWarning("CreateGameDTO is null. Aborting creation.");
                    throw new ArgumentNullException(nameof(createDto), "CreateGameDTO cannot be null");
                }

                Game? game = _gameMapper.ToEntity(createDto);

                // Assign a unique GUID using the GuidGenerator utility
                game.Id = await _guidGenerator.GenerateUniqueGuidAsync(ExistsById);

                var createdGame = await _gameRepository.CreateAsync(game);

                _logger.LogInformation("Game with ID: {GameId} successfully created.", game.Id);

                return _gameMapper.ToDTO(createdGame);
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
                var game = await _context.Set<Game>().FirstOrDefaultAsync(u => u.Id == id);

                if (game == null)
                {
                    _logger.LogWarning("Game with ID: {GameId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Game with ID: {GameId} found. Updating fields...", id);

                // Buď ručně:
                _gameMapper.ToExistingEntity(updateDto, game);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Game with ID: {GameId} successfully updated.", id);

                return _gameMapper.ToDTO(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Game with ID: {GameId}", id);
                throw;
            }
        }
    }
}
