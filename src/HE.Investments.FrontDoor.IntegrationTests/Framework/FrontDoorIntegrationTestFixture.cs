using HE.Investments.FrontDoor.WWW;
using HE.Investments.FrontDoor.WWW.Config;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

public class FrontDoorIntegrationTestFixture : IntegrationTestFixture<Program>
{
    private readonly Lazy<IServiceScope> _scope;

    public FrontDoorIntegrationTestFixture()
    {
        _scope = new Lazy<IServiceScope>(() => Server.Services.CreateScope());
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_scope.IsValueCreated)
        {
            _scope.Value.Dispose();
        }
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<LoanApplicationConfig, Config.LoanApplicationConfig>();
    }
}
