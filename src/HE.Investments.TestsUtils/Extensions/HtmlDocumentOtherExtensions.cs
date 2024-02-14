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

    public static IHtmlDocument HasWarning(this IHtmlDocument htmlDocument, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetElements("div.govuk-warning-text").Where(x => x.TextContent.Contains(text)).ToList();

        BasicHtmlDocumentExtensions.ValidateExist(filtered, "govuk-warning", text, exist);

        if (exist)
        {
            var warning = filtered.Single();
            warning.QuerySelectorAll("span.govuk-warning-text__assistive").Should().ContainSingle();

            var textNodes = warning.QuerySelectorAll("strong.govuk-warning-text__text");

            textNodes.Should().ContainSingle();
            textNodes.Single().TextContent.Should().Be($"Warning{text}");
        }

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

    public static IHtmlDocument HasParagraph(this IHtmlDocument htmlDocument, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetElements("p.govuk-body", text);

        BasicHtmlDocumentExtensions.ValidateExist(filtered, "govuk-paragraph", text, exist);

        return htmlDocument;
    }

    public static IHtmlDocument IsEmpty(this IHtmlDocument htmlDocument)
    {
        var body = htmlDocument.GetElementsByTagName("body").FirstOrDefault();

        body.Should().NotBeNull();
        body!.ChildElementCount.Should().Be(0, "Document is not empty.");

        return htmlDocument;
    }

    public static IHtmlDocument HasSummaryItem(this IHtmlDocument htmlDocument, string label, string? text = null, bool exists = true)
    {
        var summaryRow = htmlDocument.QuerySelectorAll($"[data-testId='{label.ToLowerInvariant().Replace(' ', '-')}-summary']");
        if (!exists)
        {
            summaryRow.Should().BeEmpty($"There should be no summary item with label {label}");
            return htmlDocument;
        }

        summaryRow.Should().ContainSingle($"There should be one summary item with label {label}");
        var paragraphs = summaryRow.Single().Descendants<IHtmlParagraphElement>().Where(x => text == null || x.TextContent == text).ToList();

        paragraphs.Should().ContainSingle($"There should be one summary item with text {text}");

        return htmlDocument;
    }

    public static IHtmlDocument HasSummaryDetails(this IHtmlDocument htmlDocument, string text)
    {
        var details = htmlDocument.GetElementsByClassName("govuk-details")
            .Where(d => d.Children.Any(c => c.TextContent.Contains(text) && c.ClassName == "govuk-details__text"));

        details.Should().ContainSingle("Only one element with class 'govuk-details' should exist");

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

    public static IHtmlDocument HasSelectListItem(this IHtmlDocument htmlDocument, string text, string? description)
    {
        var exist = htmlDocument.GetElements(".govuk-list li")
            .Any(item =>
                item.QuerySelectorAll(".govuk-body a b").Any(t => t.TextContent.Contains(text)) &&
                (string.IsNullOrWhiteSpace(description) || item.QuerySelectorAll(".govuk-body").Any(t => t.TextContent.Contains(description))));

        exist.Should().BeTrue($"There is no SelectList item with text: '{text}' and description: '{description}'");

        return htmlDocument;
    }

    public static IHtmlDocument HasPagination(this IHtmlDocument htmlDocument)
    {
        var exist = htmlDocument.GetElements(".govuk-pagination")
            .Any();

        exist.Should().BeTrue($"There is no Pagination element");

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
