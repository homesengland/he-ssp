using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class BasicHtmlDocumentExtensions
{
    public static IHtmlDocument HasElementWithText(this IHtmlDocument htmlDocument, string tagName, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetLastChildByTagAndText(tagName, text);

        ValidateExist(filtered, text, tagName, exist);

        return htmlDocument;
    }

    public static IList<IElement> GetElements(this IHtmlDocument htmlDocument, string selector, string? text = null)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [.. htmlDocument.QuerySelectorAll(selector)];
        }

        return FilterByText(htmlDocument.QuerySelectorAll(selector), text);
    }

    internal static IList<IElement> GetLastChildByTagAndText(this IHtmlDocument htmlDocument, string tagName, string text)
    {
        var elements = htmlDocument.GetElementsByTagName(tagName);

        return FilterByText(elements, text);
    }

    internal static void ValidateExist(IList<IElement> elements, string elementType, string text, bool exist)
    {
        if (exist)
        {
            elements.Count.Should().Be(1, $"Only one element '{elementType}' with innerText {text} should exist");
        }
        else
        {
            elements.Count.Should().Be(0, $"Element '{elementType}' with innerText {text} should not exist");
        }
    }

    private static List<IElement> FilterByText(IEnumerable<IElement> elements, string text)
    {
        return elements.Where(e => e.ChildElementCount == 0 && e.TextContent.Contains(text))
            .ToList();
    }
}
