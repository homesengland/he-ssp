using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public AhpCrmContext AhpCrmContext => Scope.Value.ServiceProvider.GetRequiredService<AhpCrmContext>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<AhpCrmContext>();
    }
}
