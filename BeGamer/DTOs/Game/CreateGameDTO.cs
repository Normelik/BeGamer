using BeGamer.Enums;
using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.Game
{
    public record CreateGameDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; init; } = string.Empty;

        [Required(ErrorMessage = "Number of minimum players is required")]
        [Range(1, 100, ErrorMessage = "Minimum number of players must be between 1 and 100.")]
        public int MinPlayers { get; init; }

        [Required(ErrorMessage = "Number of maximum players is required")]
        [Range(1, 100, ErrorMessage = "Maximum number of players must be between 1 and 100.")]
        public int MaxPlayers { get; init; }

        [Required(ErrorMessage = "Type of game is required")]
        public BoardGameType Type { get; init; }
    }
}
