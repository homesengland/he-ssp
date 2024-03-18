using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.MockedServices;
using HE.Investments.Loans.WWW;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework;

public class LoansIntegrationTestFixture : IntegrationTestFixture<Program>
{
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.Decorate<IPrefillDataRepository, PrefillDataRepositoryMock>();
    }
}
