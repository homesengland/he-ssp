using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.FeatureManagement;

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

    protected IFeatureManager FeatureManager => _fixture.FeatureManager;

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

        if (currentPage is null && !string.IsNullOrEmpty(pageUrl))
        {
            return await TestClient.NavigateTo(pageUrl);
        }

        if (!string.IsNullOrEmpty(pageUrl) && currentPage is not null)
        {
            var path = new Uri(currentPage.Url).AbsolutePath;
            if (path == null || !path.EndsWith(pageUrl, StringComparison.InvariantCulture))
            {
                return await TestClient.NavigateTo(pageUrl);
            }
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

    protected async Task<IHtmlDocument> TestQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        // given
        var continueButton = await GivenTestQuestionPage(startPageUrl, expectedPageTitle);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            inputs);

        // then
        ThenTestQuestionPage(nextPage, expectedPageUrlAfterContinue);
        return nextPage;
    }

    protected async Task<IHtmlButtonElement> GivenTestQuestionPage(string startPageUrl, string expectedPageTitle)
    {
        var currentPage = await GetCurrentPage(startPageUrl);
        currentPage
            .UrlWithoutQueryEndsWith(startPageUrl)
            .HasTitle(expectedPageTitle)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        return continueButton;
    }

    protected void ThenTestQuestionPage(IHtmlDocument nextPage, string expectedPageUrlAfterContinue)
    {
        nextPage.UrlWithoutQueryEndsWith(expectedPageUrlAfterContinue);
        SaveCurrentPage();
    }

    private IntegrationTestClient InitializeTestClient(IntegrationTestFixture<TProgram> fixture)
    {
        if (_fixture.DataBag.TryGetValue(nameof(TestClient), out var testClient))
        {
            return ((IntegrationTestClient)testClient).AsLoggedUser();
        }

        var newTestClient = new IntegrationTestClient(fixture.CreateClient(), _fixture.LoginData);
        _fixture.DataBag[nameof(TestClient)] = newTestClient;
        return newTestClient;
    }
}
