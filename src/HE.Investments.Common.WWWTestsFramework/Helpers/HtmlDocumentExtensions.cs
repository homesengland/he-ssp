using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace HE.Investments.Common.WWWTestsFramework.Helpers;

public static class HtmlDocumentExtensions
{
    public static IList<IElement> GetLastChildByTagAndText(this IHtmlDocument htmlDocument, string tagName, string text)
    {
        var elements = htmlDocument.GetElementsByTagName(tagName);

        return elements.Where(e => e.ChildElementCount == 0 && e.TextContent.Contains(text)).ToList();
    }

    public static IList<IElement> GetValidationErrors(this IHtmlDocument htmlDocument, string forName)
    {
        return htmlDocument.QuerySelectorAll($"span[data-valmsg-for=\"{forName}\"]").ToList();
    }

    public static IList<IElement> GetNavigationAnchors(this IHtmlDocument htmlDocument, string href)
    {
        return htmlDocument.QuerySelectorAll($"a[href=\"#{href}\"]").ToList();
    }

    public static IList<IElement> GetHintElements(this IHtmlDocument htmlDocument, string text)
    {
        return htmlDocument.QuerySelectorAll("div.govuk-hint")
            .Where(e => e.ChildElementCount == 0 && e.TextContent.Contains(text))
            .ToList();
    }
}
