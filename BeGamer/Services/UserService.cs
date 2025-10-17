using BeGamer.Data;
using BeGamer.DTOs.User;
using BeGamer.Mappers;
using BeGamer.Services.Interfaces;
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
        
        //CREATE USER
        public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("Creating a new user with username: {Username}", createUserDTO.Username);
            try
            {
                var user = _userMapper.ToModel(createUserDTO);
                user.Id = Guid.NewGuid(); // Assign a new GUID

                // Check the originality of the generated GUID
                while (true) {
                    if (await UserExistsById(user.Id)) break;
                    user.Id = Guid.NewGuid();
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
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
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

        // GET USER
        public async Task<UserDTO> GetUserById(Guid id)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);

            try
            {
                await UserExistsById(id); // Check if user exists

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
        public async Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDTO)
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
                user.Username = updateUserDTO.Username;
                user.Nickname = updateUserDTO.Nickname;

                // nebo pomocí mapperu:
                // _mapper.Map(updateUserDTO, user);

                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} successfully updated.", id);

                return _userMapper.ToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with ID: {UserId}", id);
                throw; // přeposílá výjimku dál
            }
        }

        // DELETE USER
        public async Task<bool> DeleteUser(Guid id)
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

        public async Task<bool> UserExistsById(Guid id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}
