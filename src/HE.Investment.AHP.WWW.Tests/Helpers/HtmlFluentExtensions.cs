using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investment.AHP.WWW.Tests.Helpers;

public static class HtmlFluentExtensions
{
    public static IHtmlDocument HasElementWithText(this IHtmlDocument htmlDocument, string tagName, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetLastChildByTagAndText(tagName, text);

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

    public static IHtmlDocument HasInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(1, $"Only one element input with name {fieldName} should exist");

        if (!string.IsNullOrEmpty(label))
        {
            var labels = htmlDocument.GetLastChildByTagAndText("label", label);
            labels.Count.Should().Be(1, $"Only one element input with label with innerText {label} should exist");
        }

        if (!string.IsNullOrEmpty(value))
        {
            inputs.First().InnerHtml.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasRadio(this IHtmlDocument htmlDocument, string fieldName, IList<string> options)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(options.Count, $"{options.Count} inputs with name {fieldName} should exist");

        return htmlDocument;
    }

    public static IHtmlDocument IsEmpty(this IHtmlDocument htmlDocument)
    {
        var body = htmlDocument.GetElementsByTagName("body").FirstOrDefault();

        body.Should().NotBeNull();
        body!.ChildElementCount.Should().Be(0, "Document is not empty.");

        return htmlDocument;
    }

    public static IHtmlDocument HasErrorMessage(this IHtmlDocument htmlDocument, string fieldName, string? text = null, bool exist = true)
    {
        var filtered = htmlDocument.GetValidationErrors(fieldName);

        AssertErrorMessage(fieldName, $"Error:{text}", exist, filtered);

        return htmlDocument;
    }

    public static IHtmlDocument HasSummaryErrorMessage(this IHtmlDocument htmlDocument, string fieldName, string? text = null, bool exist = true)
    {
        var filtered = htmlDocument.GetNavigationAnchors(fieldName);

        AssertErrorMessage(fieldName, text, exist, filtered);

        return htmlDocument;
    }

    public static IHtmlDocument HasSummaryDetails(this IHtmlDocument htmlDocument, string text)
    {
        var details = htmlDocument.GetElementsByClassName("govuk-details")
            .Where(d => d.Children.Any(c => c.TextContent.Contains(text) && c.ClassName == "govuk-details__text"));

        details.Should().NotBeNull($"Only one element with class 'govuk-details' should exist");

        return htmlDocument;
    }

    private static void AssertErrorMessage(string fieldName, string? text, bool exist, IList<IElement> filtered)
    {
        if (exist)
        {
            filtered.Count.Should().Be(1, $"Only one validation error should exist for {fieldName}");
            filtered.Single().TextContent.Should().Contain(text);
        }
        else
        {
            filtered.Count.Should().Be(0, $"Validation error for {fieldName} should not exist");
        }
    }
}
