using AngleSharp.Dom;
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
        return new ListCardModel(title!.TextContent.Trim(), listCardHeaderAction, listCard.GetListCardContent());
    }

    public static IList<ListCardContent> GetListCardContent(this IElement listCard)
    {
        var content = listCard.GetElementsByClassName("govuk-summary-card__content").SingleOrDefault();
        content.Should().NotBeNull("There is no Content element on ListCard");

        var listCardContent = new List<ListCardContent>();

        foreach (var contentItem in content!.GetElementsByClassName("govuk-summary-card__content-item"))
        {
            var contentTitle = contentItem.GetElementsByClassName("govuk-heading-s").SingleOrDefault()!.TextContent.Trim();
            var contentDescription = contentItem.GetElementsByTagName("p").SingleOrDefault();
            var contentDescriptionText = contentDescription?.TextContent.Trim();

            listCardContent.Add(new ListCardContent(contentTitle, contentDescriptionText, GetListCardContentItems(contentItem)));
        }

        return listCardContent;
    }

    private static List<ListCardContentItem> GetListCardContentItems(IElement contentItem)
    {
        var listCardContentItems = new List<ListCardContentItem>();
        foreach (var row in contentItem.GetElementsByClassName("govuk-summary-list__row"))
        {
            var contentItemItemLink = row.GetElementsByTagName("a").OfType<IHtmlAnchorElement>().SingleOrDefault();
            listCardContentItems.Add(new ListCardContentItem(contentItemItemLink!.TextContent.Trim(), contentItemItemLink.Href));
        }

        return listCardContentItems;
    }
}
