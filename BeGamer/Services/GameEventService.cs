using BeGamer.Data;
using BeGamer.DTOs.GameEvent;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BeGamer.Services
{
    public class GameEventService : IGameEventService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GameEventService> _logger;
        private readonly GameEventMapper _gameEventMapper;
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly GuidGenerator _guidGenerator;

        public GameEventService(AppDbContext context, ILogger<GameEventService> logger, GameEventMapper gameEventMapper, IUserService userService, IJwtTokenService jwtTokenService, GuidGenerator guidGenerator)
        {
            _context = context;
            _logger = logger;
            _gameEventMapper = gameEventMapper;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _guidGenerator = guidGenerator;
        }

        // CREATE EVENT
        public async Task<GameEventDTO> CreateGameEvent(CreateGameEventDTO createGameEventDTO)
        {
            _logger.LogInformation("Start creating new GameEvent.");

            
            try
            {
                var gameEvent = _gameEventMapper.ToEntity(createGameEventDTO);

                // Assign a unique GUID using the GuidGenerator utility
                gameEvent.Id = _guidGenerator.GenerateUniqueGuid(GameEventExistsById);

                await _context.GameEvents.AddAsync(gameEvent);
                await _context.SaveChangesAsync();
                _logger.LogInformation("GameEvent with ID: {GameEventId} successfully created.", gameEvent.Id);

                return _gameEventMapper.ToDTO(gameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new GameEvent.");
                throw;
            }
        }

        // GET ALL EVENTS
        public async Task<IEnumerable<GameEventDTO>> GetAllGameEventsAsync()
        {

            _logger.LogInformation("Fetching all GameEvents from the database.");

            try
            {
                var gameEvents = await _context.GameEvents.ToListAsync();
                _logger.LogInformation("Fetched {Count} GameEvents from the database.", gameEvents.Count);

                if (gameEvents.IsNullOrEmpty())
                {
                    return Enumerable.Empty<GameEventDTO>();
                }

                return _gameEventMapper.ToDtoList(gameEvents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching gameEvents.");
                throw;
            }
        }

        // GET EVENT BY ID
        public async Task<GameEventDTO> GetGameEventById(Guid id)
        {
            _logger.LogInformation("Fetching GameEvent with ID: {GameEventId}", id);

            try
            {
                GameEventExistsById(id); // Check if GameEvents exists

                var gameEvent = await _context.GameEvents.FirstOrDefaultAsync(u => u.Id == id);

                if (gameEvent == null)
                {
                    _logger.LogWarning("GameEvent with ID: {GameEventId} not found.", id);
                    return null;
                }

                _logger.LogInformation("GameEvent with ID: {GameEventId} successfully fetched.", id);
                return _gameEventMapper.ToDTO(gameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
                throw;
            }
        }

        // UPDATE EVENT
        public async Task<GameEventDTO> UpdateGameEvent(Guid id, GameEventDTO gameEventDto)
        {
            _logger.LogInformation("Attempting to update GameEvent with ID: {GameEventId}", id);
            
            try
            {
                GameEventExistsById(id); // Check if GameEvent exists

                var gameEvent = await _context.GameEvents.FirstOrDefaultAsync(u => u.Id == id);

                if (gameEvent == null)
                {
                    _logger.LogWarning("GameEvent with ID: {GameEventId} not found.", id);
                    return null;
                }

                _logger.LogInformation("GameEvent with ID: {GameEventId} found. Updating fields...", id);

                // Buď ručně:
                gameEvent.Title = gameEventDto.Title;
                gameEvent.Location = gameEventDto.Location;
                gameEvent.DateEvent = gameEventDto.DateEvent;
                gameEvent.MaxPlayers = gameEventDto.MaxPlayers;
                gameEvent.MinPlayers = gameEventDto.MinPlayers;
                gameEvent.RegistrationDeadline = gameEventDto.RegistrationDeadline;
                gameEvent.Note = gameEventDto.Note;
                gameEvent.GameId = gameEventDto.GameId;

                await _context.SaveChangesAsync();

                _logger.LogInformation("GameEvent with ID: {GameEventId} successfully updated.", id);

                return _gameEventMapper.ToDTO(gameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating GameEvent with ID: {GameEventId}", id);
                throw; 
            }
        }

        // DELETE EVENT
        public async Task<bool> DeleteGameEvent(Guid id)
        {
            try
            {
                GameEventExistsById(id); // Check if GameEvent exists

                var gameEvent = await _context.GameEvents.FindAsync(id);

                if (gameEvent is null)
                {
                    _logger.LogWarning("GameEvent with ID {GameEventId} not found for deletion.", id);
                    return false;
                }

                _context.GameEvents.Remove(gameEvent);

                var changes = await _context.SaveChangesAsync();


                if (changes > 0)
                {
                    _logger.LogInformation("GameEvent with ID {GameEventId} successfully deleted.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("GameEvent with ID {GameEventId} was found but no changes were saved.", id);
                    return false;
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting GameEvent with ID {GameEventId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting GameEvent with ID {GameEventId}.", id);
                return false;
            }
        }
        private bool GameEventExistsById(Guid id)
        {
            return _context.GameEvents.Any(e => e.Id == id);
        }
    }
}
