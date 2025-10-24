using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserById(string id);
        Task<UserDTO> CreateUserAsync(RegisterUserDTO createUserDTO);
        Task<UserDTO> UpdateUser(string id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteUser(string id);

        CustomUser GetUserAsOrganizer(string id);
        Task<CustomUser?> GetUserByUsernameAsync(string username);

        bool UserExistsById(string id);
    }
}
