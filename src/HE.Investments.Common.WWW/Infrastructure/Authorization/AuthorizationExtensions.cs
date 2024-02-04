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

    public static IServiceCollection ConfigureCookiePolicyForHeIdentity(this IServiceCollection services)
    {
#pragma warning disable format
        services.Configure(delegate(CookiePolicyOptions options)
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
        });
#pragma warning restore format
        return services;
    }

    private static void CheckSameSite(CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None && !options.Secure)
        {
            options.SameSite = SameSiteMode.Unspecified;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:Opening parenthesis should be spaced correctly", Justification = "Temp")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1310:Specify StringComparison for correctness", Justification = "Temp")]
    private static IMvcBuilder AddHeIdentityCookieAuth(this IMvcBuilder builder, HeIdentityCookieConfiguration heIdentityConfig, IWebHostEnvironment env, IEnumerable<string>? additionalScopes = null)
    {
        if (string.IsNullOrWhiteSpace(heIdentityConfig.Domain))
        {
            throw new ArgumentException("Domain must be specified");
        }

        if (string.IsNullOrWhiteSpace(heIdentityConfig.ClientId))
        {
            throw new ArgumentException("ClientId must be specified");
        }

        if (string.IsNullOrWhiteSpace(heIdentityConfig.ClientSecret))
        {
            throw new ArgumentException("ClientSecret must be specified");
        }

        builder.Services.AddSingleton((HeIdentityConfiguration)heIdentityConfig);
        builder.Services.ConfigureCookiePolicyForHeIdentity();
        builder.Services.AddAuthentication(delegate (AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = "Cookies";
            options.DefaultSignInScheme = "Cookies";
            options.DefaultChallengeScheme = "Cookies";
        }).AddCookie(delegate (CookieAuthenticationOptions options)
        {
            options.Cookie.Name = "he_identity";
            options.Cookie.Path = "/";
            options.AccessDeniedPath = new PathString("/access-denied");
            options.LoginPath = new PathString("/signin");
            options.ReturnUrlParameter = "redirectUri";
            options.Events.OnSigningOut = async context =>
            {
                var revokeRefreshTokenHandler = context.HttpContext.RequestServices.GetRequiredService<RevokeRefreshTokenHandler>();
                await revokeRefreshTokenHandler.RevokeRefreshTokenAsync(await context.HttpContext.GetTokenAsync("refresh_token"));
            };
            options.Events.OnValidatePrincipal = context =>
            {
                var requiredService = context.HttpContext.RequestServices.GetRequiredService<TokenRefreshHandler>();
                return requiredService.EnsureTokenRefreshAsync(new CookieValidatePrincipalContextFacade(context), DateTimeOffset.UtcNow);
            };
        }).AddOpenIdConnect("HeIdentity", delegate (OpenIdConnectOptions options)
        {
            options.Authority = "https://" + heIdentityConfig.Domain;
            options.ClientId = heIdentityConfig.ClientId;
            options.ClientSecret = heIdentityConfig.ClientSecret;
            options.ResponseType = "code";
            options.ResponseMode = "query";
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Scope.Add("offline_access");
            if (additionalScopes != null)
            {
                foreach (var additionalScope in additionalScopes)
                {
                    options.Scope.Add(additionalScope);
                }
            }

            if (heIdentityConfig.CallbackPath != null)
            {
                options.CallbackPath = new PathString(heIdentityConfig.CallbackPath);
            }
            else
            {
                options.CallbackPath = new PathString("/callback");
            }

            options.ClaimsIssuer = "Auth0";
            options.SaveTokens = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                RoleClaimType = "http://homesengland.org.uk/claims/roles",
            };
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context =>
                {
                    if (context.Properties.Items.ContainsKey("screen_hint"))
                    {
                        context.ProtocolMessage.SetParameter("screen_hint", context.Properties.Items["screen_hint"]);
                    }

                    if (!string.IsNullOrWhiteSpace(heIdentityConfig.Audience))
                    {
                        context.ProtocolMessage.SetParameter("audience", heIdentityConfig.Audience);
                    }

                    return Task.CompletedTask;
                },
                OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var text2 = "https://" + heIdentityConfig.Domain + "/v2/logout?client_id=" + heIdentityConfig.ClientId;
                    var text3 = context.Properties.RedirectUri;
                    if (!string.IsNullOrEmpty(text3))
                    {
                        if (text3.StartsWith('/'))
                        {
                            var request = context.Request;
                            text3 = request.Scheme + "://" + request.Host + request.PathBase + text3;
                        }

                        text2 = text2 + "&returnTo=" + Uri.EscapeDataString(text3);
                    }

                    context.Response.Redirect(text2);
                    context.HandleResponse();
                    return Task.CompletedTask;
                },
                OnRemoteFailure = context =>
                {
                    var failure = context.Failure;
                    if (failure != null && failure.Message == "Correlation failed.")
                    {
                        context.Response.Redirect("/signin");
                        context.HandleResponse();
                    }

                    if (context.HttpContext.Request.Query.ContainsKey("error") && context.HttpContext.Request.Query["error"] == "unauthorized")
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        string text = context.HttpContext.Request.Query["error_description"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (text.StartsWith("VERIFY_EMAIL:"))
                        {
                            var value2 = text.Replace("VERIFY_EMAIL:", string.Empty).Trim();
                            var options2 = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Lax,
                            };
                            context.Response.Cookies.Append("emailToVerify", value2, options2);
                            context.Response.Redirect("/verify-email");
                            context.HandleResponse();
                        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }

                    return Task.CompletedTask;
                },
            };
            if (env.IsDevelopment())
            {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                };
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
            }
        });
#pragma warning disable SA1130 // Use lambda syntax
        builder.Services.AddSingleton((Func<IServiceProvider, IAuthenticationApiClient>)delegate (IServiceProvider provider)
        {
            var value = provider.GetRequiredService<IOptions<OpenIdConnectOptions>>().Value;
            var connection = new HttpClientAuthenticationConnection(value.Backchannel);
            return new AuthenticationApiClient(heIdentityConfig.Domain, connection);
        });
#pragma warning restore SA1130 // Use lambda syntax
        builder.Services.AddTransient<IClientCredentialHelper, ClientCredentialHelper>();
        builder.Services.AddSingleton(heIdentityConfig);
        builder.Services.AddSingleton<TokenRefreshHandler>();
        builder.Services.AddSingleton<RevokeRefreshTokenHandler>();
        builder.Services.AddHttpContextAccessor();
        var assembly = Assembly.GetExecutingAssembly();
        builder.AddApplicationPart(assembly);
        builder.Services.Configure(delegate (MvcRazorRuntimeCompilationOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(assembly));
        });
        builder.AddRazorRuntimeCompilation();
        return builder;
    }
}
