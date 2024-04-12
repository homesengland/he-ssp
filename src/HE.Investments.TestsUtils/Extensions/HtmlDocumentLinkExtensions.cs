using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentLinkExtensions
{
    public static IHtmlDocument HasBackLink(this IHtmlDocument htmlDocument, out IHtmlAnchorElement backLink, bool validateLink = true)
    {
        var backButton = htmlDocument.GetElementsByClassName("govuk-back-link").SingleOrDefault();
        backButton.Should().NotBeNull().And.BeAssignableTo<IHtmlAnchorElement>();
        if (validateLink)
        {
            backButton!.IsLink().Should().BeTrue();
        }

        backButton!.Text().Trim().Should().Be("Back");
        backLink = (backButton as IHtmlAnchorElement)!;

        return htmlDocument;
    }

    public static IHtmlDocument HasSaveAndReturnToApplicationLink(this IHtmlDocument htmlDocument)
    {
        var links = GetLinkButtons(htmlDocument);

        links = HtmlElementFilters.WithText(links, "Save and return to application");

        links.SingleOrDefault().Should().NotBeNull("There is no single LinkButton element on page");

        return htmlDocument;
    }

    public static IHtmlDocument HasReturnToApplicationsListLink(this IHtmlDocument htmlDocument)
    {
        var links = GetLinks(htmlDocument);

        links = HtmlElementFilters.WithText(links, "Return to applications");

        links.SingleOrDefault().Should().NotBeNull("There is no single LinkButton element on page");

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkWithHref(this IHtmlDocument htmlDocument, string href, out IHtmlAnchorElement htmlElement)
    {
        var allLinks = htmlDocument.GetElementsByTagName("a").OfType<IHtmlAnchorElement>();
        var matchingLinks = allLinks.Where(x => x.Href.EndsWith(href, StringComparison.CurrentCultureIgnoreCase)).ToList();

        matchingLinks.Should().ContainSingle($"There should be single link with href {href}");
        htmlElement = matchingLinks.Single();

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkWithTestId(this IHtmlDocument htmlDocument, string testId, out IHtmlAnchorElement htmlElement)
    {
        var element = htmlDocument.GetElementByTestId(testId);

        var anchorElement = element as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with data-testId {testId} should be IHtmlAnchorElement");
        htmlElement = anchorElement!;

        return htmlDocument;
    }

    public static IHtmlDocument HasNoElementWithTestId(this IHtmlDocument htmlDocument, string testId)
    {
        var element = htmlDocument.TryGetElementByTestId(testId);
        element.Should().BeNull($"Element with data-testId {testId} should not exist");

        return htmlDocument;
    }

    private static IList<IElement> GetLinkButtons(this IHtmlDocument htmlDocument)
    {
        return htmlDocument
            .QuerySelectorAll("button.govuk-button-link")
            .Select(i => i)
            .ToList();
    }

    private static IList<IElement> GetLinks(this IHtmlDocument htmlDocument)
    {
        return htmlDocument
            .QuerySelectorAll("a.govuk-link")
            .Select(i => i)
            .ToList();
    }
}
