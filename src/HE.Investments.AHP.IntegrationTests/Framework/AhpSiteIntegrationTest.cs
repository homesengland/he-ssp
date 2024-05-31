using AngleSharp.Html.Dom;
using HE.Investments.TestsUtils.Extensions;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpSiteIntegrationTest : AhpIntegrationTest
{
    protected AhpSiteIntegrationTest(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    protected Task<IHtmlDocument> TestSiteQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        Action<IHtmlDocument>[] additionalChecksForExpectedPage =
        [
            page => page.HasSaveAndReturnToSiteListButton(),
            page => page.HasTitleCaption(SiteData.SiteName),
        ];

        return TestQuestionPage(
            startPageUrl,
            expectedPageTitle,
            expectedPageUrlAfterContinue,
            additionalChecksForExpectedPage,
            inputs);
    }
}
