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
    }

    public IDictionary<string, object> DataBag { get; }

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
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            x.AddSingleton<IUserConfig, UserConfig>(y => y.GetRequiredService<IConfiguration>().GetSection("IntegrationTestsConfig:User").Get<UserConfig>());
        });
        base.ConfigureWebHost(builder);
    }
}
