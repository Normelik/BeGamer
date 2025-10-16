namespace BeGamer.DTOs
{
    public record UserDTO(
        Guid Id,
        string Username,
        string? Nickname
    )
    {
    }
}
