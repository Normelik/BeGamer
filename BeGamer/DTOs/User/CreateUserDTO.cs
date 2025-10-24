using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.User
{
    public record CreateUserDTO(
        string Username,
        string Password,
        string? Nickname
    ) : RegisterUserDTO(Username, Password)
    {
    }
}
