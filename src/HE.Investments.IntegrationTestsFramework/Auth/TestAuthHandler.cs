using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HE.Investments.IntegrationTestsFramework.Auth;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";

    public const string HeaderUserGlobalId = "UserGlobalId";

    public const string HeaderUserEmail = "UserEmail";

#pragma warning disable CS0618 // Type or member is obsolete
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }
#pragma warning restore CS0618 // Type or member is obsolete

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue(HeaderUserGlobalId, out var userGlobalId))
        {
            return Task.FromResult(AuthenticateResult.Fail("User Global Id is not provided"));
        }

        Context.Request.Headers.TryGetValue(HeaderUserEmail, out var emailAddress);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.NameIdentifier, userGlobalId!),
            new Claim(ClaimTypes.Email, emailAddress!),
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
