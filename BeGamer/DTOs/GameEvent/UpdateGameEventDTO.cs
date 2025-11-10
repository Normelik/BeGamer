namespace BeGamer.DTOs.GameEvent
{
    public record UpdateGameEventDTO(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        Guid LocationId
    )
    {}
}
