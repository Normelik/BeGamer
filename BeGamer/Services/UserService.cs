using BeGamer.Data;
using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserMapper _userMapper;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, UserMapper userMapper, ILogger<UserService> logger)
        {
            _context = context;
            _userMapper = userMapper;
            _logger = logger;
        }

        //CREATE USER
        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("Creating a new user with username: {Username}", createUserDTO.Username);
            try
            {
                var user = _userMapper.ToModel(createUserDTO);
                user.Id = Guid.NewGuid().ToString(); // Assign a new GUID

                // Check the originality of the generated GUID
                while (true)
                {
                    
                    if (!UserExistsById(user.Id)) break;
                    user.Id = Guid.NewGuid().ToString();
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID: {UserId} successfully created.", user.Id);

                return _userMapper.ToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                throw;
            }
        }

        //GET ALL USERS
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users from the database.");

            try
            {
                var users = await _context.Users.ToListAsync();
                _logger.LogInformation("Fetched {Count} users from the database.", users.Count);

                return _userMapper.ToDTOList(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                throw;
            }
        }

        // GET USER BY ID
        public async Task<UserDTO> GetUserById(string id)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);

            try
            {
                UserExistsById(id); // Check if user exists

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", id);
                    return null;
                }

                _logger.LogInformation("User with ID: {UserId} successfully fetched.", id);
                return _userMapper.ToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
                throw;
            }
        }

        // UPDATE USER
        public async Task<UserDTO> UpdateUser(string id, UpdateUserDTO updateUserDTO)
        {
            _logger.LogInformation("Attempting to update user with ID: {UserId}", id);

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", id);
                    return null;
                }

                _logger.LogInformation("User with ID: {UserId} found. Updating fields...", id);

                // Buď ručně:
                user.UserName = updateUserDTO.Username;
                user.Nickname = updateUserDTO.Nickname;

                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} successfully updated.", id);

                return _userMapper.ToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with ID: {UserId}", id);
                throw;
            }
        }

        // DELETE USER
        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                UserExistsById(id); // Check if user exists

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

        public bool UserExistsById(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public CustomUser GetUserAsOrganizer(string id)
        {
            try
            {

                // Check if user exists
                UserExistsById(id);
                var user = _context.Users
                    .Include(u => u.OrganizedEvents)
                    .FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID: {UserId} not found.", id);
                    return null;
                }
                _logger.LogInformation("User with ID: {UserId} successfully fetched as organizer.", id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user as organizer with ID: {UserId}", id);
                throw;
            }
        }
    }
}
