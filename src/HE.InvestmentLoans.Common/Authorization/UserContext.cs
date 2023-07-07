using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HE.InvestmentLoans.Common.Authorization;

public class UserContext : IUserContext
{
    private const string EmailClaimName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

    private const string IdentifierClaimName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    public UserContext(HttpContext httpContext)
    {
        if (httpContext == null || httpContext.User == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var httpUser = httpContext.User;
        Email = GetClaimValue(httpUser, EmailClaimName);
        UserGlobalId = GetRequiredClaimValue(httpUser, IdentifierClaimName);
    }

    public string UserGlobalId { get; init; }

    public string? Email { get; init; }


    private string GetRequiredClaimValue(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return GetClaimValue(claimsPrincipal, claimType) ?? throw new ArgumentNullException(claimType);
    }

    private string? GetClaimValue(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
    }
}
