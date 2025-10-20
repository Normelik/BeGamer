using BeGamer.DTOs;
using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserById(Guid id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO);
        Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteUser(Guid id);

        User GetUserAsOrganizer(Guid id);

        bool UserExistsById(Guid id);
    }
}
