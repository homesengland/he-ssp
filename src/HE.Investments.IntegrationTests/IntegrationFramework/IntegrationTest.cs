using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.Loans;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

[Collection(nameof(IntegrationTestSharedContext))]
public class IntegrationTest
{
    private readonly IntegrationTestFixture<Program> _fixture;

    protected IntegrationTest(IntegrationTestFixture<Program> fixture)
    {
        _fixture = fixture;
        UserConfig = _fixture.Services.GetRequiredService<IUserConfig>();
        TestClient = new IntegrationTestClient(fixture.CreateClient(), new IntegrationTestConfig(UserConfig));
    }

    protected IntegrationTestClient TestClient { get; }

    protected IUserConfig UserConfig { get; }

    protected void SetSharedData<T>(string key, T data)
        where T : notnull
    {
        _fixture.DataBag[key] = data;
    }

    protected T GetSharedData<T>(string key)
        where T : class
    {
#if DEBUG
        if (key == SharedKeys.ApplicationLoanIdInDraftStatusKey)
        {
            return ("20a97aa8-6e50-ee11-be6f-002248c652b4" as T)!;
        }
#endif

        return (_fixture.DataBag[key] as T)!;
    }
}
