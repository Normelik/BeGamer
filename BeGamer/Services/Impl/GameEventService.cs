using AutoMapper;
using BeGamer.DTOs.GameEvent;
using BeGamer.Models;
using BeGamer.Repositories.Interfaces;
using BeGamer.Services.common;
using BeGamer.Services.Interfaces;
using BeGamer.Utils;

namespace BeGamer.Services
{
    public class GameEventService : BaseAppService<GameEvent, GameEventDTO, CreateGameEventDTO, UpdateGameEventDTO>, IGameEventService
    {

        private readonly IAddressRepository _addressRepository;
        private readonly IGameEventRepository _gameEventRepository;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public GameEventService(
            IGameEventRepository genericRepository,
            IAddressRepository addressRepository,
            IMapper mapper,
            GuidGenerator guidGenerator,
            ILogger<GameEventService> logger,
            IGameEventRepository gameEventRepository,
            IUserService userService,
            IAuthService authService)
            : base(guidGenerator, mapper, genericRepository, logger)
        {
            _addressRepository = addressRepository;
            _gameEventRepository = gameEventRepository;
            _userService = userService;
            _authService = authService;
        }

        //// UPDATE EVENT
        //public async Task<GameEventDTO?> UpdateGameEvent(Guid id, UpdateGameEventDTO updateGameEventDTO)
        //{
        //    _logger.LogInformation("Attempting to update GameEvent with ID: {GameEventId}", id);

        //    try
        //    {
        //        await ExistsById(id); // Check if GameEvent exists

        //        //var gameEvent = await _genericRepository.FindByIdAsync(id);
        //        var gameEvent = await _gameEventRepository.GetGameEventByIdWithParticipantsAsync(id);

        //        if (gameEvent == null)
        //        {
        //            _logger.LogWarning("GameEvent with ID: {GameEventId} not found.", id);
        //            return null;
        //        }

        //        _logger.LogInformation("GameEvent with ID: {GameEventId} found. Updating fields...", id);

        //        // Update fields
        //        _mapper.Map(updateGameEventDTO, gameEvent);



        //        await _genericRepository.SaveChangesAsync();

        //        _logger.LogInformation("GameEvent with ID: {GameEventId} successfully updated.", id);

        //        return _mapper.Map<GameEventDTO>(gameEvent);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating GameEvent with ID: {GameEventId}", id);
        //        throw;
        //    }
        //}

        //// GET ALL EVENTS BY DISTANCE
        //public async Task<IEnumerable<GameEvent>> GetNearbyGameEvents(
        //                                    double userLatitude,
        //                                    double userLongitude,
        //                                    double distanceInMeters)
        //{
        //    double R = 6371000; // Poloměr Země v metrech
        //    double latRad = userLatitude * Math.PI / 180;
        //    double lonRad = userLongitude * Math.PI / 180;

        //    var nearbyEvents = await _genericRepository.GetAllAsync();
        // TODO: query to get only events within distanceInMeters
        //var nearbyEvents = await _context.GameEvents
        //    .Include(e => e.Location)
        //    .Where(e => e.Location != null &&
        //        (
        //            2 * R * Math.Asin(
        //                Math.Sqrt(
        //                    Math.Pow(Math.Sin(((e.Location.Latitude * Math.PI / 180) - latRad) / 2), 2) +
        //                    (Math.Cos(latRad) * Math.Cos(e.Location.Latitude * Math.PI / 180) *
        //                    Math.Pow(Math.Sin(((e.Location.Longitude * Math.PI / 180) - lonRad) / 2), 2))
        //                )
        //            )
        //        ) <= distanceInMeters
        //    )
        //    .ToListAsync();

        //    return nearbyEvents;
        //}

        public async override Task<GameEventDTO> CreateAsync(CreateGameEventDTO createDto)
        {
            _logger.LogInformation("Start creating new GameEvent.");

            try
            {
                GameEvent gameEvent = _mapper.Map<GameEvent>(createDto);

                // Assign a unique GUID using the GuidGenerator utility
                gameEvent.Id = await _guidGenerator.GenerateUniqueGuidAsync(ExistsById);

                // Get the assigned user (organizer)
                CustomUser? assignedUser = await _authService.GetAssignedUserAsync();
                if (assignedUser == null)
                {
                    _logger.LogError("Assigned user is null. Cannot create GameEvent without organizer.");
                    throw new InvalidOperationException("Assigned user is null. Cannot create GameEvent without organizer.");
                }
                gameEvent.OrganizerId = assignedUser.Id;

                gameEvent.Location = await _addressRepository.FindByIdAsync(createDto.LocationId);

                await _genericRepository.CreateAsync(gameEvent);
                _logger.LogInformation("GameEvent with ID: {GameEventId} successfully created.", gameEvent.Id);

                return _mapper.Map<GameEventDTO>(gameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new GameEvent.");
                throw;
            }
        }

        public async override Task<GameEventDTO> UpdateAsync(Guid id, UpdateGameEventDTO updateDto)
        {
            _logger.LogInformation("Attempting to update GameEvent with ID: {GameEventId}", id);

            try
            {
                await ExistsById(id); // Check if GameEvent exists

                var gameEvent = await _genericRepository.FindByIdAsync(id);

                if (gameEvent == null)
                {
                    _logger.LogWarning("GameEvent with ID: {GameEventId} not found.", id);
                    return null;
                }

                _logger.LogInformation("GameEvent with ID: {GameEventId} found. Updating fields...", id);

                // Update fields
                _mapper.Map(updateDto, gameEvent);

                await _genericRepository.SaveChangesAsync();

                _logger.LogInformation("GameEvent with ID: {GameEventId} successfully updated.", id);

                return _mapper.Map<GameEventDTO>(gameEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating GameEvent with ID: {GameEventId}", id);
                throw;
            }
        }
    }
}
