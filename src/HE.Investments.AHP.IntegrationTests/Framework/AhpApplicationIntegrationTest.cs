using AngleSharp.Html.Dom;
using HE.Investments.TestsUtils.Extensions;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.Framework;

public class AhpApplicationIntegrationTest : AhpIntegrationTest
{
    protected AhpApplicationIntegrationTest(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    protected Task<IHtmlDocument> TestApplicationQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        Action<IHtmlDocument>[] additionalChecksForExpectedPage =
        [
            page => page.HasReturnOrSaveAndReturnToApplicationLink(),
            page => page.HasTitleCaption(ApplicationData.ApplicationName),
        ];

        return TestQuestionPage(
            startPageUrl,
            expectedPageTitle,
            expectedPageUrlAfterContinue,
            additionalChecksForExpectedPage,
            inputs);
    }
}
