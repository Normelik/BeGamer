using BeGamer.Data;
using BeGamer.DTOs;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class GameEventService : IGameEventService
    {
        private readonly AppDbContext _context;

        public GameEventService(AppDbContext context)
        {
            _context = context;
        }
        public async IEnumerable<GameEventDto> getAllEvents()
        {
            var existingEvents = await _context.GameEvents.ToListAsync();

            if (existingEvents == null || !existingEvents.Any())
            {
                return Enumerable.Empty<GameEventDto>();
            }

            return GameEventMapper.ToDtoList(existingEvents);
        }
    }
}
