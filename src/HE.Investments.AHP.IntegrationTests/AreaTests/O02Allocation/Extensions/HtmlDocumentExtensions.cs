using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Extensions;

public static class HtmlDocumentExtensions
{
    public static IHtmlDocument HasCancelAndReturnToAllocationLink(this IHtmlDocument htmlDocument)
    {
        var links = htmlDocument.GetLinkButtons();

        links = HtmlElementFilters.WithText(links, "Cancel and return to allocation");
        links.SingleOrDefault().Should().NotBeNull("There is no single LinkButton element on page");

        return htmlDocument;
    }

    public static IHtmlDocument HasNoMilestoneStatusTag(this IHtmlDocument htmlDocument, MilestoneType milestoneType)
    {
        return htmlDocument.HasNoElementWithTestId($"{milestoneType.ToString().ToIdTag()}-milestone-status");
    }

    public static IHtmlDocument HasMilestoneStatusTag(this IHtmlDocument htmlDocument, MilestoneType milestoneType, MilestoneStatus status)
    {
        return htmlDocument.HasStatusTagByTestId(status.GetDescription(), $"{milestoneType.ToString().ToIdTag()}-milestone-status");
    }

    public static IElement HasSummaryCardSection(this IHtmlDocument htmlDocument, string sectionName)
    {
        htmlDocument.HasElementForTestId($"{sectionName.ToIdTag()}-card-section", out var cardSection);

        return cardSection;
    }
}
