using BeGamer.DTOs;
using BeGamer.DTOs.User;

namespace BeGamer.Services.Interfaces
{
    public interface IAuthService
    {

        string Login(LoginDTO loginDTO);
        string Register(CreateUserDTO registerUserDTO);
    }
}
