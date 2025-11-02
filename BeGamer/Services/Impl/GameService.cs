using BeGamer.Data;
using BeGamer.DTOs.Game;
using BeGamer.Mappers;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GameService> _logger;
        private readonly GameMapper _gameMapper;
        private readonly GuidGenerator _guidGenerator;

        public GameService(
                                AppDbContext context,
                                ILogger<GameService> logger,
                                GameMapper gameMapper,
                                GuidGenerator guidGenerator)
        {
            _context = context;
            _logger = logger;
            _gameMapper = gameMapper;
            _guidGenerator = guidGenerator;
        }

        // GET ALL GAMES
        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {

            _logger.LogInformation("Fetching all Games from the database.");

            try
            {
                var games = await _context.Game.ToListAsync();
                _logger.LogInformation("Fetched {Count} Games from the database.", games.Count);

                if (games == null || !games.Any())
                {
                    return Enumerable.Empty<GameDTO>();
                }

                return _gameMapper.ToDtoList(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching games.");
                throw;
            }
        }

        // GET GAME BY ID
        public async Task<GameDTO?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching Game with ID: {GameId}", id);

            try
            {
                ExistsById(id); // Check if Games exists

                var game = await _context.Game.FirstOrDefaultAsync(u => u.Id == id);

                if (game == null)
                {
                    _logger.LogWarning("Game with ID: {GameId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Game with ID: {GameId} successfully fetched.", id);
                return _gameMapper.ToDTO(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
                throw;
            }
        }

        // CREATE GAME
        public async Task<GameDTO> CreateAsync(CreateGameDTO createDto)
        {
            _logger.LogInformation("Start creating new Game.");

            try
            {
                var game = _gameMapper.ToEntity(createDto);

                // Assign a unique GUID using the GuidGenerator utility
                game.Id = _guidGenerator.GenerateUniqueGuid(ExistsById);

                await _context.Game.AddAsync(game);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Game with ID: {GameId} successfully created.", game.Id);

                return _gameMapper.ToDTO(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new Game.");
                throw;
            }
        }

        // UPDATE GAME
        public async Task<GameDTO> UpdateAsync(Guid id, UpdateGameDTO updateDto)
        {
            _logger.LogInformation("Attempting to update Game with ID: {GameId}", id);

            try
            {
                var game = await _context.Game.FirstOrDefaultAsync(u => u.Id == id);

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

        // DELETE GAME
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete Game with ID: {GameId}", id);

            try
            {
                ExistsById(id); // Check if Game exists

                var game = await _context.Game.FindAsync(id);

                if (game is null)
                {
                    _logger.LogWarning("Game with ID {GameId} not found for deletion.", id);
                    return false;
                }

                _context.Game.Remove(game);

                var changes = await _context.SaveChangesAsync();


                if (changes > 0)
                {
                    return true; // deleted successfully
                }
                else
                {
                    _logger.LogWarning("Game with ID {GameId} was found but no changes were saved.", id);
                    return false;
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting Game with ID {GameId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting Game with ID {GameId}.", id);
                return false;
            }
        }

        public bool ExistsById(Guid id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
