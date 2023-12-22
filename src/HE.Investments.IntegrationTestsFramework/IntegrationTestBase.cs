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
        TestClient = new IntegrationTestClient(fixture.CreateClient(), _fixture.LoginData);
    }

    protected IntegrationTestClient TestClient { get; }

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

    protected void SetCurrentPage(IHtmlDocument page)
    {
        SetSharedData(CommonKeys.CurrentPageKey, page);
    }

    protected async Task<IHtmlDocument> GetCurrentPage(string navigateTo)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(CommonKeys.CurrentPageKey);

        if (currentPage is null)
        {
            return await TestClient.NavigateTo(navigateTo);
        }

        return currentPage;
    }

    protected async Task<IHtmlDocument> GetCurrentPage(Func<Task<IHtmlDocument>> alternativeNavigate)
    {
        var currentPage = GetSharedDataOrNull<IHtmlDocument>(CommonKeys.CurrentPageKey);

        if (currentPage is null)
        {
            return await alternativeNavigate();
        }

        return currentPage;
    }
}
