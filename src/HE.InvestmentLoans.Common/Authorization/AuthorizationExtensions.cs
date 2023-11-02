using He.Identity.Auth0;
using He.Identity.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HE.InvestmentLoans.Common.Authorization;

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
        serviceCollections.AddScoped<IUserContext, UserContext>(x => new UserContext(x.GetRequiredService<IHttpContextAccessor>()!.HttpContext!));
    }
}
