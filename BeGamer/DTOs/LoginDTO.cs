using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs
{
    public record LoginDTO(
        [Required(ErrorMessage = "Username is required")]
        string Username,
        [Required(ErrorMessage = "Password is required")]
        string Password)
    {
    }
}
