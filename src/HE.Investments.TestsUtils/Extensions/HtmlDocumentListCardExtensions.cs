using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.TestsUtils.Extensions.Models;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentListCardExtensions
{
    public static ListCardModel GetFirstListCard(this IHtmlDocument htmlDocument)
    {
        var listCard = htmlDocument.GetElementsByClassName("govuk-summary-card").FirstOrDefault();
        listCard.Should().NotBeNull("There is no ListCard element on page");

        var title = listCard!.GetElementsByClassName("govuk-summary-card__title").SingleOrDefault();
        title.Should().NotBeNull("There is no Title element on ListCard");

        var action = listCard.GetElementsByClassName("govuk-summary-card__action").SingleOrDefault();
        var actionLink = action?.GetElementsByTagName("a").OfType<IHtmlAnchorElement>().SingleOrDefault();

        var listCardHeaderAction = action is null ? null : new ListCardHeaderAction(actionLink!.TextContent.Trim(), actionLink.Href);
        return new ListCardModel(title!.TextContent.Trim(), listCardHeaderAction);
    }
}
