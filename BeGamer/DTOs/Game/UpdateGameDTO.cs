using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.Game
{
    public record UpdateGameDTO(
        [Required(ErrorMessage ="Title is required")]
        string Title,
        [Required(ErrorMessage ="Number of minimum players is required")]
        int MinPlayers,
        [Required(ErrorMessage ="Number of maximum players is required")]
        int MaxPlayers,
        [Required(ErrorMessage ="Type of game is required")]
        int Type)
    {
    }
}