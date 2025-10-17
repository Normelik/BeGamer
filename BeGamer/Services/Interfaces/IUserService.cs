using BeGamer.DTOs.User;

namespace BeGamer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(Guid id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO);
        Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDTO);
        Task<bool> DeleteUser(Guid id);

        Task<bool> UserExistsById(Guid id);
    }
}
