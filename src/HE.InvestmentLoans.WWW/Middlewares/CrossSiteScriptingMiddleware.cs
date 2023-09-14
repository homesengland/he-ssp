using System.Text;
using HE.InvestmentLoans.Common.Infrastructure;

namespace HE.InvestmentLoans.WWW.Middlewares;

public class CrossSiteScriptingMiddleware
{
    private readonly RequestDelegate _next;

    public CrossSiteScriptingMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context, NonceModel nonce)
    {
        var contenSecurityPolicy = new StringBuilder();

        contenSecurityPolicy.Append("default-src 'self';");
#pragma warning disable CA1305 // Specify IFormatProvider
        contenSecurityPolicy.Append($"script-src 'strict-dynamic' 'nonce-{nonce.GaNonce}' ");
#pragma warning restore CA1305 // Specify IFormatProvider
        contenSecurityPolicy.Append("'self' https://www.google-analytics.com;");
        contenSecurityPolicy.Append("object-src 'none';");
        contenSecurityPolicy.Append("base-uri 'none';");
        contenSecurityPolicy.Append("referrer 'none'");

        const string csp = "Content-Security-Policy";
        if (context.Response.Headers.All(x => x.Key != csp))
        {
            context.Response.Headers.Add(csp, contenSecurityPolicy.ToString());
        }

        const string referrerPolicy = "Referrer-Policy";
        if (context.Response.Headers.All(x => x.Key != referrerPolicy))
        {
            context.Response.Headers.Add(referrerPolicy, "none");
        }

        await _next(context);
    }
}
