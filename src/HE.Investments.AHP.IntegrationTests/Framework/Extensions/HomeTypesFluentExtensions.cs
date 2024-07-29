using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Framework.Extensions;

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

    public static IList<string> GetHomeTypeIds(this IHtmlDocument homeTypeListPage)
    {
        return homeTypeListPage.GetHiddenListInput("HomeTypes", "HomeTypeId").ToList();
    }

    public static HomeTypeDetails GetHomeTypeDetails(this IHtmlDocument homeTypeListPage, string homeTypeId)
    {
        var numberOfHomes = homeTypeListPage.GetElementByTestId($"number-of-homes-{homeTypeId}").Text().Trim();
        homeTypeListPage.HasLinkWithTestId($"home-type-{homeTypeId}", out var editHomeTypeButton);

        return new HomeTypeDetails(
            homeTypeId,
            editHomeTypeButton.Text.Trim(),
            int.TryParse(numberOfHomes, out var number) ? number : 0);
    }
}
