
using System.ComponentModel.DataAnnotations;

namespace BeGamer.DTOs.GameEvent
{
    public record CreateGameEventDTO(
    [Required(ErrorMessage = "Title is required")]
    string Title,
    [Required(ErrorMessage = "Location Id is required")]
    Guid LocationId,
    [Required(ErrorMessage = "Date and time is required")]
    DateTime DateEvent,
    [Required(ErrorMessage = "Number of minimum players is required")]
    [Range(1, 100, ErrorMessage = "Minimum number of players must be between 1 and 100.")]
    int MaxPlayers,
    [Required(ErrorMessage = "Number of maximum players is required")]
    [Range(1, 100, ErrorMessage = "Maximum number of players must be between 1 and 100.")]
    int MinPlayers,
    DateOnly RegistrationDeadline,
    string Note,
    [Required(ErrorMessage = "Game Id is required")]
    Guid GameId
);
}


