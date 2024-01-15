using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class AhpHtmlFluentExtensions
{
    public static IHtmlDocument HasValidApplicationReferenceNumber(this IHtmlDocument htmlDocument)
    {
        var referenceNumber = htmlDocument.GetElementById("reference-number");
        referenceNumber.Should().NotBeNull();
        referenceNumber!.TextContent.Should().MatchRegex("ID (\\d{7}$)");
        return htmlDocument;
    }

    public static IHtmlDocument HasChangeAnswerSummaryButton(this IHtmlDocument htmlDocument, string summaryName, out IHtmlAnchorElement changeAnswerButton)
    {
        var summaryRow = htmlDocument.GetElementByTestId($"{summaryName.ToIdTag()}-summary");
        var changeLink = summaryRow.FindDescendant<IHtmlAnchorElement>();
        changeLink.Should().NotBeNull();

        changeLink!.Text.Trim().Should().Be("Change");
        changeAnswerButton = changeLink!;

        return htmlDocument;
    }
}
