using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.User
{
    public record CreateUserDTO(
        [Required(ErrorMessage ="Username is required")]
        string Username,
        [Required(ErrorMessage ="Password is required")]
        string Password,
        string? Nickname
    )
    {
    }
}
