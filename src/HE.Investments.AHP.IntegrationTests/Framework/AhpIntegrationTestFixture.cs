using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.IntegrationTestsFramework;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public AhpApplicationCrmContext AhpApplicationCrmContext => Scope.Value.ServiceProvider.GetRequiredService<AhpApplicationCrmContext>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<AhpApplicationCrmContext>();
    }
}
