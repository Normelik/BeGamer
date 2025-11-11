using AutoMapper;
using BeGamer.Data;
using BeGamer.DTOs.Auth;
using BeGamer.DTOs.User;
using BeGamer.Mappers;
using BeGamer.Models;
using BeGamer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeGamer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<CustomUser> _passwordHasher;

        public UserService(
            AppDbContext context,
            IMapper mapper,
            ILogger<UserService> logger,
            IPasswordHasher<CustomUser> passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        //CREATE USER
        public async Task<UserDTO> CreateUserAsync(RegisterUserDTO registerUserDTO)
        {
            _logger.LogInformation("Creating a new user with username: {Username}", registerUserDTO.Username);
            try
            {
                var user = _mapper.Map<CustomUser>(registerUserDTO);
                user.Id = Guid.NewGuid(); // Assign a new GUID
                user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDTO.Password);

                // Check the originality of the generated GUID
                while (true)
                {

                    if (!UserExistsById(user.Id)) break;
                    user.Id = Guid.NewGuid();
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID: {UserId} successfully created.", user.Id);

                return _mapper.Map<UserDTO>(user);
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

                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                throw;
            }
        }

        // GET USER BY ID
        public async Task<UserDTO> GetUserById(Guid id)
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
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
                throw;
            }
        }
        // Generická metoda s mapovací funkcí
        //public async Task<T> GetByIdAsync<T>(string id, Func<CustomUser, T> map)
        //{
        //    // Načteme entitu spolu s organizátorem
        //    var user = await _context.GameEvents
        //                                  .Include(e => e.Organizer)
        //                                  .FirstOrDefaultAsync(e => e.Id.Equals(id));

        //    if (user == null)
        //        throw new KeyNotFoundException($"GameEvent s Id {id} nebyl nalezen.");

        //    // Použijeme mapovací funkci pro převod na DTO, entitu nebo jiný typ
        //    return map(user);
        //}

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

                _mapper.Map(updateUserDTO, user);

                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} successfully updated.", id);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with ID: {UserId}", id);
                throw;
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

        public bool UserExistsById(Guid id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public CustomUser GetUserAsOrganizer(Guid id)
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
        public async Task<CustomUser?> GetUserByUsernameAsync(string username)
        {
            _logger.LogInformation("Fetching user with username: {Username}", username);
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    _logger.LogWarning("User with username: {Username} not found.", username);
                    return null;
                }
                _logger.LogInformation("User with username: {Username} successfully fetched.", username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user with username: {Username}", username);
                throw;
            }
        }
    }
}
