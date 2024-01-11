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

    public async Task TestQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        // given
        var currentPage = await GetCurrentPage(startPageUrl);
        currentPage
            .UrlEndWith(startPageUrl)
            .HasTitle(expectedPageTitle)
            .HasGdsBackButton()
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            inputs);

        // then
        nextPage.UrlEndWith(expectedPageUrlAfterContinue);
        SaveCurrentPage();
    }
}
