using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.User
{
    public record UpdateUserDTO(
        [Required]
        string Username,
        string? Nickname)
    {
    }
}
