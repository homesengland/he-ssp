using AngleSharp.Dom;
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

    public static IHtmlDocument HasInput(this IHtmlDocument htmlDocument, string fieldName, string? text = null)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(1, $"Only one element input with name {fieldName} should exist");

        if (!string.IsNullOrEmpty(text))
        {
            var labels = htmlDocument.GetElementsByTagAndText("label", text);
            labels.Count.Should().Be(1, $"Only one element input with label with innerText {text} should exist");
        }

        return htmlDocument;
    }

    public static IHtmlDocument IsEmpty(this IHtmlDocument htmlDocument)
    {
        var body = htmlDocument.GetElementsByTagName("body").FirstOrDefault();

        body.Should().NotBeNull();
        body!.ChildElementCount.Should().Be(0, "Document is not empty.");

        return htmlDocument;
    }
}
