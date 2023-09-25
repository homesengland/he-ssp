using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTestFixture<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public IntegrationTestFixture()
    {
        DataBag = new Dictionary<string, object>();

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json", true).Build();

        var userConfig = configuration.GetSection("IntegrationTestsConfig:UserConfig").Get<UserConfig>();

        if (userConfig is not null)
        {
            UserData = new IntegrationUserData(userConfig);
        }
        else
        {
            UserData = new IntegrationUserData();
        }
    }

    public IDictionary<string, object> DataBag { get; }

    public IntegrationUserData UserData { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x => x.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { }));

        base.ConfigureWebHost(builder);
    }
}
