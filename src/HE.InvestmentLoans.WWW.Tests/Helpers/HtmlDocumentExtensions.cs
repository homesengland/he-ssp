using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace HE.InvestmentLoans.WWW.Tests.Helpers;

public static class HtmlDocumentExtensions
{
    public static IList<IElement> GetElementsByTagAndText(this IHtmlDocument htmlDocument, string tagName, string text)
    {
        var elements = htmlDocument.GetElementsByTagName(tagName);

        return elements.Where(e => e.ChildElementCount == 0 && e.TextContent.Contains(text)).ToList();
    }
}
