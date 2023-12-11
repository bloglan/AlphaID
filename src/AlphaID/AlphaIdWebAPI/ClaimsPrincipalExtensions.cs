using IdentityModel;
using System.Security.Claims;

namespace AlphaIdWebAPI;

public static class ClaimsPrincipalExtensions
{
    public static string? ClientId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(JwtClaimTypes.ClientId);
    }
}
