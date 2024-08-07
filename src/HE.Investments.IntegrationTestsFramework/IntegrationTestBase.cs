using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework.Config;
using HE.Investments.IntegrationTestsFramework.Utils;
using HE.Investments.TestsUtils.Extensions;
using Xunit;

namespace HE.Investments.IntegrationTestsFramework;

public class IntegrationTestBase<TProgram> : IAsyncLifetime
    where TProgram : class
{
    private readonly IntegrationTestFixture<TProgram> _fixture;

    protected IntegrationTestBase(IntegrationTestFixture<TProgram> fixture)
    {
        _fixture = fixture;
        TestClient = InitializeTestClient(fixture);
    }

    protected IntegrationTestClient TestClient { get; }

    protected DateTimeManipulator DateTimeManipulator => _fixture.DateTimeManipulator;

    public virtual async Task InitializeAsync()
    {
        var prerequisiteCheckResult = GetSharedDataOrNull<IList<string>>(CommonKeys.PrerequisiteCheckResult) ?? await _fixture.VerifyPrerequisites();
        SetSharedData(CommonKeys.PrerequisiteCheckResult, prerequisiteCheckResult);

        if (prerequisiteCheckResult.Count > 0)
        {
            var errorBuilder = new StringBuilder();
            errorBuilder.AppendLine("Integration tests prerequisite check failed:");

            foreach (var result in prerequisiteCheckResult)
            {
                errorBuilder.AppendLine(CultureInfo.InvariantCulture, $" - {result}");
            }

            Assert.Fail(errorBuilder.ToString());
        }
    }

    public virtual Task DisposeAsync()
    {
        DateTimeManipulator.ResetTimeTravel();

        return Task.CompletedTask;
    }

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

    protected T ReturnSharedData<T>(Action<T>? action = null)
        where T : class, new()
    {
        var data = GetSharedDataOrNull<T>(typeof(T).ToString());
        if (data is null)
        {
            data = new T();
            action?.Invoke(data);
            SetSharedData(typeof(T).ToString(), data);
        }

        return data;
    }

    protected T GetSharedData<T>(string key)
    {
        return (T)_fixture.DataBag[key];
    }

    protected T? GetSharedDataOrNull<T>(string key)
    {
        return _fixture.DataBag.TryGetValue(key, out var data) ? (T)data : default;
    }

    [SuppressMessage("Design", "CA1031", Justification = "We want to catch all exceptions and retry check within some time")]
    protected async Task<TimeSpan> WaitFor(Func<Task<bool>> waitCondition, TimeSpan timeout, TimeSpan? refreshDelay = null)
    {
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            var errorDetails = "no details";
            try
            {
                if (await waitCondition())
                {
                    return stopwatch.Elapsed;
                }
            }
            catch (Exception ex)
            {
                errorDetails = ex.Message;
            }

            await Task.Delay(refreshDelay ?? TimeSpan.FromSeconds(1));

            if (stopwatch.Elapsed > timeout)
            {
                throw new TimeoutException($"Wait condition did not become true within the timeout of {timeout.TotalSeconds} seconds, {errorDetails}.");
            }
        }
    }

    protected async Task<IHtmlDocument> TestQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        Action<IHtmlDocument>[] additionalChecksForExpectedPage,
        params (string InputName, string Value)[] inputs)
    {
        // given
        var continueButton = await GivenTestQuestionPage(startPageUrl, expectedPageTitle, additionalChecksForExpectedPage);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            inputs);

        // then
        ThenTestQuestionPage(nextPage, expectedPageUrlAfterContinue);
        return nextPage;
    }

    protected async Task<IHtmlDocument> TestQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        return await TestQuestionPage(startPageUrl, expectedPageTitle, expectedPageUrlAfterContinue, [], inputs);
    }

    protected async Task<IHtmlButtonElement> GivenTestQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        Action<IHtmlDocument>[]? additionalChecks = null)
    {
        var currentPage = await GetCurrentPage(startPageUrl);
        currentPage
            .UrlWithoutQueryEndsWith(startPageUrl)
            .HasTitle(expectedPageTitle)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        foreach (var additionalCheck in additionalChecks ?? [])
        {
            additionalCheck(currentPage);
        }

        return continueButton;
    }

    protected void ThenTestQuestionPage(IHtmlDocument nextPage, string expectedPageUrlAfterContinue)
    {
        nextPage.HasNoValidationErrors()
            .UrlWithoutQueryEndsWith(expectedPageUrlAfterContinue);

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
