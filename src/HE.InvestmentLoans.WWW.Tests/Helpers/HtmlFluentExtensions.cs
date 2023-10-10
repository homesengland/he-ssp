using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.InvestmentLoans.WWW.Tests.Helpers;

public static class HtmlFluentExtensions
{
    public static IHtmlDocument HasElementWithText(this IHtmlDocument htmlDocument, string tagName, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetElementsByTagAndText(tagName, text);

        if (exist)
        {
            filtered.Count.Should().Be(1, $"Only one element '{tagName}' with innerText {text} should exist");
        }
        else
        {
            filtered.Count.Should().Be(0, $"Element '{tagName}' with innerText {text} should not exist");
        }

        return htmlDocument;
    }
}
