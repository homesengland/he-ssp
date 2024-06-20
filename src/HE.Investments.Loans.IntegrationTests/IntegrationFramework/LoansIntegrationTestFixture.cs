using HE.Investments.FrontDoor.IntegrationTests.Utils;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.Crm;
using HE.Investments.Loans.WWW;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

public class LoansIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public LoanApplicationCrmRepository LoanApplicationCrmRepository => Scope.Value.ServiceProvider.GetRequiredService<LoanApplicationCrmRepository>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddFrontDoorManipulator();
        services.AddScoped<LoanApplicationCrmRepository>();
    }
}
