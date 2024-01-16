using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentOtherExtensions
{
    public static IHtmlDocument HasHint(this IHtmlDocument htmlDocument, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetElements("div.govuk-hint", text);

        BasicHtmlDocumentExtensions.ValidateExist(filtered, "govuk-hint", text, exist);

        return htmlDocument;
    }

    public static IHtmlDocument HasPageHeader(this IHtmlDocument htmlDocument, string? caption = null, string? header = null)
    {
        if (!string.IsNullOrWhiteSpace(caption))
        {
            htmlDocument
                .HasElementWithText("span", caption);
        }

        if (!string.IsNullOrWhiteSpace(header))
        {
            htmlDocument
                .HasElementWithText("h1", header);
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

    public static IHtmlDocument HasSummaryDetails(this IHtmlDocument htmlDocument, string text)
    {
        var details = htmlDocument.GetElementsByClassName("govuk-details")
            .Where(d => d.Children.Any(c => c.TextContent.Contains(text) && c.ClassName == "govuk-details__text"));

        details.Should().NotBeNull($"Only one element with class 'govuk-details' should exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasSummaryErrorMessage(this IHtmlDocument htmlDocument, string fieldName, string? text = null, bool exist = true)
    {
        var filtered = htmlDocument.GetNavigationAnchors(fieldName);

        AssertErrorMessage(fieldName, text, exist, filtered);

        return htmlDocument;
    }

    public static IHtmlDocument HasErrorMessage(this IHtmlDocument htmlDocument, string fieldName, string? text = null, bool exist = true)
    {
        var filtered = htmlDocument.GetValidationErrors(fieldName);

        AssertErrorMessage(fieldName, $"Error:{text}", exist, filtered);

        return htmlDocument;
    }

    internal static void AssertErrorMessage(string fieldName, string? text, bool exist, IList<IElement> filtered)
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

    private static IList<IElement> GetValidationErrors(this IHtmlDocument htmlDocument, string forName)
    {
        return htmlDocument.QuerySelectorAll($"span[data-valmsg-for=\"{forName}\"]").ToList();
    }

    private static IList<IElement> GetNavigationAnchors(this IHtmlDocument htmlDocument, string href)
    {
        return htmlDocument.QuerySelectorAll($"a[href=\"#{href}\"]").ToList();
    }
}
