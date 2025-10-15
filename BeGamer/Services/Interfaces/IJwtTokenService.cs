using System.Security.Claims;

namespace BeGamer.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
