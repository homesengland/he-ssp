using HE.InvestmentLoans.IntegrationTests.Config;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Helpers.DataPackages;
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
        UserData = _fixture.UserData;
        TestClient = new IntegrationTestClient(fixture.CreateClient(), UserData);
    }

    protected IntegrationTestClient TestClient { get; }

    protected IntegrationUserData UserData { get; }

    protected void SetSharedData<T>(string key, T data)
        where T : notnull
    {
        _fixture.DataBag[key] = data;
    }

    protected T GetSharedData<T>(string key)
    {
        return (T)_fixture.DataBag[key];
    }

    protected T? GetSharedDataOrNull<T>(string key)
    {
        return _fixture.DataBag.TryGetValue(key, out var data) ? (T)data : default;
    }
}
