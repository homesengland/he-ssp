using HE.Investments.FrontDoor.WWW;
using HE.Investments.FrontDoor.WWW.Config;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

public class FrontDoorIntegrationTestFixture : IntegrationTestFixture<Program>
{
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<LoanApplicationConfig, Config.LoanApplicationConfig>();
    }
}
