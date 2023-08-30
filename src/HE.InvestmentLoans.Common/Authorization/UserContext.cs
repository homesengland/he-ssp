using System.Security.Claims;
using HE.InvestmentLoans.Common.Extensions;
using Microsoft.AspNetCore.Http;

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
        SetIsAuthenticated(httpUser);
        Email = httpUser.GetClaimValue(EmailClaimName) ?? string.Empty;
        if (IsAuthenticated == true)
        {
            UserGlobalId = GetRequiredClaimValue(httpUser, IdentifierClaimName);
        }
    }

    public string UserGlobalId { get; }

    public string Email { get; }

    public bool? IsAuthenticated { get; private set; }

    private string GetRequiredClaimValue(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.GetClaimValue(claimType) ?? throw new ArgumentNullException(claimType);
    }

    private void SetIsAuthenticated(ClaimsPrincipal claimsPrincipal)
    {
        IsAuthenticated = claimsPrincipal.Identity?.IsAuthenticated;
    }
}
