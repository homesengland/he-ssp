using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentLinkExtensions
{
    public static IHtmlDocument HasGdsBackLink(this IHtmlDocument htmlDocument, bool validateLink = true)
    {
        var backButton = htmlDocument.GetElementsByClassName("govuk-back-link").SingleOrDefault();
        backButton.Should().NotBeNull();
        if (validateLink)
        {
            backButton!.IsLink().Should().BeTrue();
        }

        backButton!.Text().Trim().Should().Be("Back");
        return htmlDocument;
    }

    public static IHtmlDocument HasSaveAndReturnToApplicationLink(this IHtmlDocument htmlDocument)
    {
        var links = GetLinkButtons(htmlDocument);

        links = HtmlElementFilters.WithText(links, "Save and return to application");

        links.SingleOrDefault().Should().NotBeNull("There is no single LinkButton element on page");

        return htmlDocument;
    }

    private static IList<IElement> GetLinkButtons(this IHtmlDocument htmlDocument)
    {
        return htmlDocument
            .QuerySelectorAll("button.govuk-button-link")
            .Select(i => i)
            .ToList();
    }
}
