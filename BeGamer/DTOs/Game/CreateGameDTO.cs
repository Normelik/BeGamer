using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.Game
{
    public record CreateGameDTO(
        [Required(ErrorMessage ="Title is required")]
        string Title,
        [Required(ErrorMessage ="Number of minimum players is required")]
        [Range(1, 100, ErrorMessage = "Minimum number of players must be between 1 and 100.")]
        int MinPlayers,
        [Required(ErrorMessage ="Number of maximum players is required")]
        [Range(1, 100, ErrorMessage = "Maximum number of players must be between 1 and 100.")]
        int MaxPlayers,
        [Required(ErrorMessage ="Type of game is required")]
        int Type)
    {
    }
}
