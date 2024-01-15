using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class HomeTypesFluentExtensions
{
    public static IHtmlDocument HasDuplicateHomeTypeLink(this IHtmlDocument htmlDocument, string homeTypeId, out IHtmlAnchorElement duplicateButton)
    {
        return htmlDocument.HasLinkWithTestId($"duplicate-{homeTypeId}", out duplicateButton);
    }

    public static IHtmlDocument HasHomeTypeItem(this IHtmlDocument htmlDocument, out string homeTypeId, out string homeTypeName)
    {
        htmlDocument.HasElementForTestId("home-types-table", out var table);
        var firstHomeTypeLink = table.FindDescendant<IHtmlAnchorElement>();
        firstHomeTypeLink.Should().NotBeNull();
        firstHomeTypeLink!.GetAttribute("data-testid").Should().NotBeNull();

        homeTypeId = firstHomeTypeLink.GetAttribute("data-testid")!.Replace("home-type-", string.Empty);
        homeTypeName = firstHomeTypeLink.Text.Trim();

        return htmlDocument;
    }

    public static IHtmlDocument HasHomeTypeItem(
        this IHtmlDocument htmlDocument,
        string homeTypeId,
        string homeTypeName,
        out IHtmlAnchorElement editHomeTypeButton)
    {
        htmlDocument.HasLinkWithTestId($"home-type-{homeTypeId}", out editHomeTypeButton);
        if (!string.IsNullOrEmpty(homeTypeName))
        {
            editHomeTypeButton.Text.Trim().Should().Be(homeTypeName);
        }

        return htmlDocument;
    }
}
