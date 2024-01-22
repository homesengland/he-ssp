using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    protected AhpIntegrationTest(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        var applicationData = GetSharedDataOrNull<ApplicationData>(nameof(ApplicationData));
        if (applicationData is null)
        {
            applicationData = new ApplicationData();
            SetSharedData(nameof(ApplicationData), applicationData);
        }

        ApplicationData = applicationData;
        fixture.CheckUserLoginData();
    }

    public ApplicationData ApplicationData { get; }

    public async Task<IHtmlDocument> TestQuestionPage(
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
            .HasGdsBackButton()
            .HasGdsSubmitButton("continue-button", out var continueButton);

        return continueButton;
    }

    protected void ThenTestQuestionPage(IHtmlDocument nextPage, string expectedPageUrlAfterContinue)
    {
        nextPage.UrlWithoutQueryEndsWith(expectedPageUrlAfterContinue);
        SaveCurrentPage();
    }
}
