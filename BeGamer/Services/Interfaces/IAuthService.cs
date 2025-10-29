using BeGamer.DTOs;
using BeGamer.DTOs.Auth;

namespace BeGamer.Services.Interfaces
{
    public interface IAuthService
    {

        Task<ResponseTokenDTO?> LoginAsync(LoginDTO loginDTO);
        Task<string> RegisterAsync(RegisterUserDTO registerUserDTO);
    }
}
