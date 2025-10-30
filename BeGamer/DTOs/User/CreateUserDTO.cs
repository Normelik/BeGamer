using BeGamer.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.User
{
    public record CreateUserDTO(
        [Required(ErrorMessage ="Username is required")]
        string Username,
        string Password,
        string? Nickname
    ) : RegisterUserDTO(Username, Password)
    {
    }
}
