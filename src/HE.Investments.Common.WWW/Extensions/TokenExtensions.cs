using System.Security.Claims;

namespace HE.Investments.Common.WWW.Extensions;

public static class TokenExtensions
{
    public static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
    }
}
