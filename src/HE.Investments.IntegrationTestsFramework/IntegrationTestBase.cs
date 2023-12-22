using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework.Config;

namespace HE.Investments.IntegrationTestsFramework;

public class IntegrationTestBase<TProgram>
    where TProgram : class
{
    private readonly IntegrationTestFixture<TProgram> _fixture;

    protected IntegrationTestBase(IntegrationTestFixture<TProgram> fixture)
    {
        _fixture = fixture;
        TestClient = InitializeTestClient(fixture);
    }

    protected IntegrationTestClient TestClient { get; }

    protected void SaveCurrentPage()
    {
        SetSharedData(CommonKeys.CurrentPageKey, TestClient.CurrentPage);
    }

    protected async Task<IHtmlDocument> GetCurrentPage(string? pageUrl = null)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(CommonKeys.CurrentPageKey);
        if (currentPage is null && string.IsNullOrEmpty(pageUrl))
        {
            throw new InvalidOperationException("Current page is not set and pageUrl is not provided");
        }

        if (!string.IsNullOrEmpty(pageUrl) && (!currentPage?.Url.EndsWith(pageUrl, StringComparison.InvariantCulture) ?? true))
        {
            return await TestClient.NavigateTo(pageUrl);
        }

        if (currentPage is null)
        {
            throw new InvalidOperationException("Current page is not set");
        }

        return currentPage;
    }

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

    private IntegrationTestClient InitializeTestClient(IntegrationTestFixture<TProgram> fixture)
    {
        if (_fixture.DataBag.TryGetValue(nameof(TestClient), out var testClient))
        {
            return (IntegrationTestClient)testClient;
        }
        else
        {
            var newTestClient = new IntegrationTestClient(fixture.CreateClient(), _fixture.LoginData);
            _fixture.DataBag[nameof(TestClient)] = newTestClient;
            return newTestClient;
        }
    }
}
