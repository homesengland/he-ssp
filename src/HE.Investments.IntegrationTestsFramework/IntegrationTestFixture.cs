using HE.Investments.Account.Api.Contract.User;
using HE.Investments.DocumentService.Configs;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.TestsUtils.TestData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.IntegrationTestsFramework;

public class IntegrationTestFixture<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public IntegrationTestFixture()
    {
        DataBag = new Dictionary<string, object>();
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json", true).Build();

        var userConfig = Configuration.GetSection("IntegrationTestsConfig:UserConfig").Get<UserLoginData>();
        LoginData = userConfig ?? new UserLoginData();
    }

    public IDictionary<string, object> DataBag { get; }

    public ILoginData LoginData { get; }

    public IConfiguration Configuration { get; }

    public void CheckUserLoginData()
    {
        if (!LoginData.IsProvided())
        {
            throw new InvalidDataException("Please set IntegrationTestsConfig:UserConfig:UserGlobalId, IntegrationTestsConfig:UserConfig:Email and IntegrationTestsConfig:UserConfig:OrganisationId in settings");
        }
    }

    public void MockUserAccount()
    {
        var profileDetails = AccountTestData.PaulSmith(LoginData.Email);
        var userOrganisations = new[] { AccountTestData.PwCAdmin(LoginData.OrganisationId) };

        IntegrationTestsHttpClientFactory.RegisterMockedClient(
            "AccountRepository",
            (HttpMethod.Get, $"api/user/{LoginData.UserGlobalId}/accounts", new AccountDetails(LoginData.UserGlobalId, LoginData.Email, userOrganisations)),
            (HttpMethod.Get, $"api/user/{LoginData.UserGlobalId}/profile", profileDetails));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            x.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, _ => { });

            x.AddSingleton<IDocumentServiceSettings, MockedDocumentServiceSettings>();
            x.Decorate<IHttpClientFactory, IntegrationTestsHttpClientFactory>();
        });

        base.ConfigureWebHost(builder);
    }

    private class MockedDocumentServiceSettings : IDocumentServiceSettings
    {
        public string Url => string.Empty;

        public bool UseMock => true;
    }
}
