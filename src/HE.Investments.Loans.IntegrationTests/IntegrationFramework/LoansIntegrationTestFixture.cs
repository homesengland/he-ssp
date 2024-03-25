using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.Crm;
using HE.Investments.Loans.WWW;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

public class LoansIntegrationTestFixture : IntegrationTestFixture<Program>
{
    private readonly Lazy<IServiceScope> _scope;

    public LoansIntegrationTestFixture()
    {
        _scope = new Lazy<IServiceScope>(() => Server.Services.CreateScope());
    }

    public FrontDoorProjectCrmRepository FrontDoorProjectCrmRepository => _scope.Value.ServiceProvider.GetRequiredService<FrontDoorProjectCrmRepository>();

    public LoanApplicationCrmRepository LoanApplicationCrmRepository => _scope.Value.ServiceProvider.GetRequiredService<LoanApplicationCrmRepository>();

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
        services.AddScoped<FrontDoorProjectCrmRepository>();
        services.AddScoped<LoanApplicationCrmRepository>();
    }
}
