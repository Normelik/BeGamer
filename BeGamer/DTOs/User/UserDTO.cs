namespace BeGamer.DTOs.User
{
    public record UserDTO(
        Guid Id,
        string Username,
        string? Nickname
    )
    {
    }
}
