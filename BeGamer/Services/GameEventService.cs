using BeGamer.Data;
using BeGamer.DTOs;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BeGamer.Services
{
    public class GameEventService : IGameEventService
    {
        private readonly AppDbContext _context;

        public GameEventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameEventDto> CreateEvent(CreateGameEventDto createGameEventDto)
        {
            if (createGameEventDto == null)
            {
                throw new ArgumentNullException(nameof(createGameEventDto));
            }
            //GameEvent newie = new GameEvent();
            //newie.Id = Guid.NewGuid();
            GameEvent newie = new GameEvent
            {
                Id = Guid.NewGuid(),
                Title = createGameEventDto.Title,
                Location = createGameEventDto.Location,
                DateEvent = createGameEventDto.DateEvent,
                Organizer = null, // To be set in the service layer
                OrganizerId = Guid.Empty, // To be set in the service layer
                MaxPlayers = createGameEventDto.MaxPlayers,
                MinPlayers = createGameEventDto.MinPlayers,
                RegistrationDeadline = createGameEventDto.RegistrationDeadline,
                Note = createGameEventDto.Note,
                GameId = createGameEventDto.GameId,
                Game = null // To be set in the service layer or elsewhere
            };
            await _context.GameEvents.AddAsync(newie);

            //GameEvent newEvent = GameEventMapper.toEntity(createGameEventDto);
            //await _context.GameEvents.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return GameEventMapper.ToDto(newie);
        }

        public Task<bool> DeleteEvent(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameEventDto>> GetAllEvents()
        {
            var existingEvents = await _context.GameEvents.ToListAsync();

            if (existingEvents.IsNullOrEmpty())
            {
                return Enumerable.Empty<GameEventDto>();
            }

            return GameEventMapper.ToDtoList(existingEvents);
        }

        public Task<GameEventDto> GetEventById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEvent(Guid id, GameEventDto gameEventDto)
        {
            throw new NotImplementedException();
        }
    }
}
