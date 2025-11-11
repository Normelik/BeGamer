using BeGamer.DTOs;
using BeGamer.DTOs.Auth;
using BeGamer.Models;

namespace BeGamer.Services.Interfaces
{
    public interface IAuthService
    {

        Task<ResponseTokenDTO?> LoginAsync(LoginDTO loginDTO);
        Task<string> RegisterAsync(RegisterUserDTO registerUserDTO);

        Task<CustomUser?> GetAssignedUserAsync();
    }
}
