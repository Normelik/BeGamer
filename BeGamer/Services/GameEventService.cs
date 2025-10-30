using BeGamer.Data;
using BeGamer.DTOs.GameEvent;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class GameEventService : IGameEventService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GameEventService> _logger;
        private readonly GameEventMapper _gameEventMapper;
        private readonly GuidGenerator _guidGenerator;

        public GameEventService(
                                AppDbContext context,
                                ILogger<GameEventService> logger,
                                GameEventMapper gameEventMapper,
                                GuidGenerator guidGenerator)
        {
            _context = context;
            _logger = logger;
            _gameEventMapper = gameEventMapper;
            _guidGenerator = guidGenerator;
        }

        // CREATE EVENT
        public async Task<GameEventDTO> CreateGameEvent(Guid id, CreateGameEventDTO createGameEventDTO)
        {
            _logger.LogInformation("Start creating new GameEvent.");

            try
            {
                var gameEvent = _gameEventMapper.ToEntity(createGameEventDTO);

                // Assign a unique GUID using the GuidGenerator utility
                gameEvent.Id = _guidGenerator.GenerateUniqueGuid(GameEventExistsById!);
                gameEvent.OrganizerId = id;

                gameEvent.Location = await _context.Addresses.FindAsync(createGameEventDTO.LocationId);

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

                if (gameEvents == null || !gameEvents.Any())
                {
                    return [];
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
        public async Task<GameEventDTO?> GetGameEventById(Guid id)
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
        public async Task<GameEventDTO?> UpdateGameEvent(Guid id, GameEventDTO gameEventDto)
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
        // GET ALL EVENTS BY DISTANCE
        public async Task<List<GameEvent>> GetNearbyGameEvents(
                                            double userLatitude,
                                            double userLongitude,
                                            double distanceInMeters)
        {
            double R = 6371000; // Poloměr Země v metrech
            double latRad = userLatitude * Math.PI / 180;
            double lonRad = userLongitude * Math.PI / 180;

            var nearbyEvents = await _context.GameEvents
                .Include(e => e.Location)
                .Where(e => e.Location != null &&
                    (
                        2 * R * Math.Asin(
                            Math.Sqrt(
                                Math.Pow(Math.Sin(((e.Location.Latitude * Math.PI / 180) - latRad) / 2), 2) +
                                (Math.Cos(latRad) * Math.Cos(e.Location.Latitude * Math.PI / 180) *
                                Math.Pow(Math.Sin(((e.Location.Longitude * Math.PI / 180) - lonRad) / 2), 2))
                            )
                        )
                    ) <= distanceInMeters
                )
                .ToListAsync();

            return nearbyEvents;
        }

    }
}
