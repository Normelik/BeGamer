using BeGamer.Data;
using BeGamer.DTOs;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserMapper _userMapper;
        private readonly ILogger<UserService> _logger;
        public UserService(AppDbContext context,UserMapper userMapper, ILogger<UserService> logger)
        {
            _context = context;
            _userMapper = userMapper;
            _logger = logger;
        }
        public Task<UserDTO> CreateUser(UserDTO userDto)
        { 
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user is null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                    return false;
                }

                _context.Users.Remove(user);

                var changes = await _context.SaveChangesAsync();

                if (changes > 0)
                {
                    _logger.LogInformation("User with ID {UserId} successfully deleted.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("User with ID {UserId} was found but no changes were saved.", id);
                    return false;
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting user with ID {UserId}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting user with ID {UserId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return _userMapper.ToDTOList(users);
        }

        public Task<UserDTO> GetUserById(Guid id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .Select(u => _userMapper.ToDTO(u))
                .FirstOrDefaultAsync();
        }

        public Task<UserDTO> UpdateUser(Guid id, UserDTO userDto)
        {
            throw new NotImplementedException();
        }
    }
}
