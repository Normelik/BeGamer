using BeGamer.DTOs;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(Guid id);
        Task<UserDTO> CreateUser(UserDTO userDto);
        Task<UserDTO> UpdateUser(Guid id, UserDTO userDto);
        Task<bool> DeleteUser(Guid id);
    }
}
