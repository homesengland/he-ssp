using System.Reflection;
using Auth0.AuthenticationApi;
using He.Identity.Auth0;
using He.Identity.Mvc;
using He.Identity.Mvc.ClientCredentials;
using He.Identity.Mvc.Extensions;
using He.Identity.Mvc.Handlers;
using HE.Investments.Common.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HE.Investments.Common.WWW.Infrastructure.Authorization;

public static class AuthorizationExtensions
{
    public static void AddIdentityProviderConfiguration(this WebApplicationBuilder builder, IMvcBuilder mvcBuilder)
    {
        var configuration = builder.Configuration;
        var auth0Config = new Auth0Config(
                            configuration["AppConfiguration:Auth0:Domain"],
                            configuration["AppConfiguration:Auth0:ClientId"],
                            configuration["AppConfiguration:Auth0:ClientSecret"]);
        var supportEmail = configuration["AppConfiguration:SupportEmail"];

        var heIdentityCookieConfiguration = new HeIdentityCookieConfiguration
        {
            Domain = auth0Config.Domain,
            ClientId = auth0Config.ClientId,
            ClientSecret = auth0Config.ClientSecret,
            SupportEmail = supportEmail,
            CookiePath = "/",
        };

        builder.Services.ConfigureHeCookieSettings(
            mvcBuilder,
            configure => configure.WithAspNetCore().WithHeIdentity());

        mvcBuilder.AddHeIdentityCookieAuth(heIdentityCookieConfiguration, builder.Environment);

        var auth0ManagementConfig = new Auth0ManagementConfig(
            auth0Config.Domain,
            auth0Config.ClientId,
            auth0Config.ClientSecret,
            configuration["AppConfiguration:Auth0:ManagementClientAudience"],
            configuration["AppConfiguration:Auth0:UserConnection"]);

        builder.Services.ConfigureIdentityManagementService(x => x.UseAuth0(auth0Config, auth0ManagementConfig));
    }

    public static void AddHttpUserContext(this IServiceCollection serviceCollections)
    {
        serviceCollections.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollections.AddScoped<IUserContext, UserContext>(x => new UserContext(x.GetRequiredService<IHttpContextAccessor>().HttpContext!));
    }
}
