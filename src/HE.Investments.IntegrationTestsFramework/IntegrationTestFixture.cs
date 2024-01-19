using HE.Investments.DocumentService.Configs;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.IntegrationTestsFramework.Config;
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

    public void ProvideLoginData(ILoginData loginData)
    {
        LoginData.Change(loginData);
    }

    public void CheckUserLoginData()
    {
        if (!LoginData.IsProvided())
        {
            throw new InvalidDataException("Please set IntegrationTestsConfig:UserConfig:UserGlobalId and IntegrationTestsConfig:UserConfig:Email in settings");
        }
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
        });

        base.ConfigureWebHost(builder);
    }

    private class MockedDocumentServiceSettings : IDocumentServiceSettings
    {
        public string Url => string.Empty;

        public bool UseMock => true;
    }
}
