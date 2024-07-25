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

    public static IHtmlDocument HasReturnOrSaveAndReturnToApplicationLink(this IHtmlDocument htmlDocument)
    {
        var links = GetLinkButtons(htmlDocument);

        var containsReturnToApplicationLink = HtmlElementFilters.ContainsElementWithText(links, "Return to applications");
        var containsSaveAndReturnToApplicationLink = HtmlElementFilters.ContainsElementWithText(links, "Save and return to application");

        (containsReturnToApplicationLink || containsSaveAndReturnToApplicationLink).Should()
            .BeTrue("There is no \"Return to application\" or \"Save and return to application\" link button");

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

    public static IHtmlDocument HasReturnToAllocationLink(this IHtmlDocument htmlDocument)
    {
        var links = GetLinks(htmlDocument);

        links = HtmlElementFilters.WithText(links, "Return to allocation");

        links.SingleOrDefault().Should().NotBeNull("There is no single LinkButton element on page");

        return htmlDocument;
    }

    public static IHtmlDocument HasSaveAndReturnToSiteListButton(this IHtmlDocument htmlDocument)
    {
        return htmlDocument.HasSaveAndReturnToSiteListButton(out _);
    }

    public static IHtmlDocument HasSaveAndReturnToSiteListButton(this IHtmlDocument htmlDocument, out IHtmlButtonElement saveAndReturnButton)
    {
        var links = GetLinkButtons(htmlDocument);

        var button = HtmlElementFilters.WithText(links, "Save and return to sites").SingleOrDefault() as IHtmlButtonElement;
        button.Should().NotBeNull("There is no Save and return to sites link button");
        button!.Type.Should().Contain($"submit");
        saveAndReturnButton = button;
        return htmlDocument;
    }

    public static IHtmlDocument HasLinkWithText(this IHtmlDocument htmlDocument, string text, out IHtmlAnchorElement link)
    {
        var links = GetLinks(htmlDocument);
        links = HtmlElementFilters.WithText(links, text);
        var matchedLink = links.SingleOrDefault();
        matchedLink.Should().NotBeNull("There is no single Link element on page");
        link = matchedLink as IHtmlAnchorElement ?? throw new InvalidOperationException("Element is not IHtmlAnchorElement");
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

    private static List<IElement> GetLinkButtons(this IHtmlDocument htmlDocument)
    {
        return htmlDocument
            .QuerySelectorAll("button.govuk-button-link")
            .Select(i => i)
            .ToList();
    }

    private static List<IElement> GetLinks(this IHtmlDocument htmlDocument)
    {
        return htmlDocument
            .QuerySelectorAll("a.govuk-link")
            .Select(i => i)
            .ToList();
    }
}
