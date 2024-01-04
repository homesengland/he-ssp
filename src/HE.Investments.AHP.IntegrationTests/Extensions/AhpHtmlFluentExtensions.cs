using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.IntegrationTestsFramework.Extensions;

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
}
