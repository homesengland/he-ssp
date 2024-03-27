using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.Crm;
using HE.Investments.Loans.WWW;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

public class LoansIntegrationTestFixture : IntegrationTestFixture<Program>
{
    public FrontDoorProjectCrmRepository FrontDoorProjectCrmRepository => Scope.Value.ServiceProvider.GetRequiredService<FrontDoorProjectCrmRepository>();

    public LoanApplicationCrmRepository LoanApplicationCrmRepository => Scope.Value.ServiceProvider.GetRequiredService<LoanApplicationCrmRepository>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped<FrontDoorProjectCrmRepository>();
        services.AddScoped<LoanApplicationCrmRepository>();
    }
}
