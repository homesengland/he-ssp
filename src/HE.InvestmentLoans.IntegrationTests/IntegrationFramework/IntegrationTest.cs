using HE.InvestmentLoans.IntegrationTests.Config;
using Xunit;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework;

public class IntegrationTest : IClassFixture<IntegrationTestFixture<Program>>
{
    private readonly IntegrationTestFixture<Program> _fixture;

    protected IntegrationTest(IntegrationTestFixture<Program> fixture)
    {
        _fixture = fixture;
        TestClient = new IntegrationTestClient(fixture.CreateClient(), new IntegrationTestConfig());
    }

    protected IntegrationTestClient TestClient { get; }

    protected void SetSharedData<T>(string key, T data)
        where T : notnull
    {
        _fixture.DataBag[key] = data;
    }

    protected T GetSharedData<T>(string key)
        where T : class
    {
        return (_fixture.DataBag[key] as T)!;
    }
}
