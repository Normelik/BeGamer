using BeGamer.DTOs;
using BeGamer.DTOs.User;

namespace BeGamer.Services.Interfaces
{
    public interface IAuthService
    {

        Task<string> Login(LoginDTO loginDTO);
        Task<string> Register(RegisterUserDTO registerUserDTO);
    }
}
