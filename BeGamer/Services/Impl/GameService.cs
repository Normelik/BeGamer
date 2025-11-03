using BeGamer.Data;
using BeGamer.DTOs.Game;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Impl.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class GameService : BaseAppService<Game, GameDTO, CreateGameDTO, UpdateGameDTO>, IGameService
    {
        public GameService(AppDbContext context, ILogger<GameService> logger, GuidGenerator guidGenerator, GameMapper gameMapper)
            : base(context, logger, guidGenerator, gameMapper)
        {
        }

        private readonly GameMapper _gameMapper;


        public override async Task<GameDTO> CreateAsync(CreateGameDTO createDto)
        {
            _logger.LogInformation("Start creating new Game.");

            try
            {
                var game = _gameMapper.ToEntity(createDto);

                // Assign a unique GUID using the GuidGenerator utility
                game.Id = await _guidGenerator.GenerateUniqueGuid(ExistsById);

                await _context.AddAsync(game);
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

        //public override async Task<bool> DeleteAsync(Guid id)
        //{
        //    _logger.LogInformation("Attempting to delete Game with ID: {GameId}", id);

        //    try
        //    {
        //        await ExistsById(id); // Check if Game exists

        //        var game = await _context.FindAsync<Game>(id);

        //        if (game is null)
        //        {
        //            _logger.LogWarning("Game with ID {GameId} not found for deletion.", id);
        //            return false;
        //        }

        //        _context.Remove(game);

        //        var changes = await _context.SaveChangesAsync();


        //        if (changes > 0)
        //        {
        //            return true; // deleted successfully
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Game with ID {GameId} was found but no changes were saved.", id);
        //            return false;
        //        }
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        _logger.LogError(dbEx, "Database update error while deleting Game with ID {GameId}.", id);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unexpected error occurred while deleting Game with ID {GameId}.", id);
        //        return false;
        //    }
        //}
        //public override async Task<IEnumerable<GameDTO>> GetAllAsync()
        //{

        //    _logger.LogInformation("Fetching all Games from the database.");

        //    try
        //    {
        //        var games = await _context.Set<Game>().ToListAsync();
        //        //var games = await _context.Game.ToListAsync();
        //        _logger.LogInformation("Fetched {Count} Games from the database.", games.Count);

        //        if (games == null || !games.Any())
        //        {
        //            return Enumerable.Empty<GameDTO>();
        //        }

        //        return _gameMapper.ToDtoList(games);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching games.");
        //        throw;
        //    }
        //}

        //public override async Task<GameDTO?> GetByIdAsync(Guid id)
        //{
        //    _logger.LogInformation("Fetching Game with ID: {GameId}", id);

        //    try
        //    {
        //        await ExistsById(id); // Check if Games exists

        //        var game = await _context.Set<Game>().FirstOrDefaultAsync(u => u.Id == id);

        //        if (game == null)
        //        {
        //            _logger.LogWarning("Game with ID: {GameId} not found.", id);
        //            return null;
        //        }

        //        _logger.LogInformation("Game with ID: {GameId} successfully fetched.", id);
        //        return _gameMapper.ToDTO(game);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
        //        throw;
        //    }
        //}

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
