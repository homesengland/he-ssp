using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investments.AHP.IntegrationTests.FillApplication;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Pages;
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
    }

    public ApplicationData ApplicationData { get; }

    public async Task TestPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        // given
        var fundingDetailsPage = await GetCurrentPage(startPageUrl);
        fundingDetailsPage
            .UrlEndWith(startPageUrl)
            .HasTitle(expectedPageTitle)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var affordabilityPage = await TestClient.SubmitButton(
            continueButton,
            inputs);

        // then
        affordabilityPage.UrlEndWith(expectedPageUrlAfterContinue);
        SaveCurrentPage();
    }
}
