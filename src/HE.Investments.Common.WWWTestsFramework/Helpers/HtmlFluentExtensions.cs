using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.Common.WWWTestsFramework.Helpers;

public static class HtmlFluentExtensions
{
    public static IHtmlDocument HasElementWithText(this IHtmlDocument htmlDocument, string tagName, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetLastChildByTagAndText(tagName, text);

        ValidateExist(filtered, text, tagName, exist);

        return htmlDocument;
    }

    public static IHtmlDocument HasInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null)
    {
        var inputs = GetAndValidateInput<IHtmlInputElement>(htmlDocument, fieldName, label);

        if (!string.IsNullOrEmpty(value))
        {
            inputs.First().Value.Should().Contain(value);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasTextAreaInput(this IHtmlDocument htmlDocument, string fieldName, string? label = null, string? value = null)
    {
        var inputs = GetAndValidateInput<IHtmlTextAreaElement>(htmlDocument, fieldName, label);

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

    public static IHtmlDocument HasCheckboxes(this IHtmlDocument htmlDocument, string fieldName, IList<string> options)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(options.Count, $"{options.Count} inputs with name {fieldName} should exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasCheckedCheckboxes(this IHtmlDocument htmlDocument, string fieldName, IList<string> checkedValues)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);

        foreach (var checkedValue in checkedValues)
        {
            var element = inputs?.Where(x => x.OuterHtml.Contains(checkedValue)).FirstOrDefault();
            var isChecked = element?.IsChecked();
            isChecked.Should().Be(true, $"{checkedValue} value should be checked");
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

    public static IHtmlDocument HasPageHeader(this IHtmlDocument htmlDocument, string caption, string header)
    {
        htmlDocument
            .HasElementWithText("span", caption)
            .HasElementWithText("h1", header);

        return htmlDocument;
    }

    public static IHtmlDocument HasFormFieldLabel(this IHtmlDocument htmlDocument, string title, string labelElement)
    {
        htmlDocument
            .HasElementWithText(labelElement, title);

        return htmlDocument;
    }

    public static IHtmlDocument HasInputHint(this IHtmlDocument htmlDocument, string text, bool exist = true)
    {
        var filtered = htmlDocument.GetHintElements(text);

        ValidateExist(filtered, "govuk-hint", text, exist);

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

    private static IEnumerable<T> GetAndValidateInput<T>(IHtmlDocument htmlDocument, string fieldName, string? label)
    {
        var inputs = htmlDocument.GetElementsByName(fieldName);
        inputs.Length.Should().Be(1, $"Only one element input with name {fieldName} should exist");

        if (!string.IsNullOrEmpty(label))
        {
            var labels = htmlDocument.GetLastChildByTagAndText("label", label);
            labels.Count.Should().Be(1, $"Only one element input with label with innerText {label} should exist");
        }

        return inputs.Cast<T>();
    }

    private static void ValidateExist(IList<IElement> elements, string elementType, string text, bool exist)
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
}
