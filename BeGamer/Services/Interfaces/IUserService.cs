using BeGamer.DTOs.Auth;
using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserById(Guid id);
        Task<UserDTO> CreateUserAsync(RegisterUserDTO createUserDTO);
        Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteUser(Guid id);

        CustomUser GetUserAsOrganizer(Guid id);
        Task<CustomUser?> GetUserByUsernameAsync(string username);

        bool UserExistsById(Guid id);
    }
}
