using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentButtonExtensions
{
    public static IHtmlDocument HasContinueButton(this IHtmlDocument htmlDocument)
    {
        GetContinueButton(htmlDocument);
        return htmlDocument;
    }

    public static IHtmlButtonElement GetContinueButton(this IHtmlDocument htmlDocument)
    {
        return GetSubmitButton(htmlDocument, "Continue");
    }

    public static IHtmlDocument HasSaveAndContinueButton(this IHtmlDocument htmlDocument)
    {
        GetSubmitButton(htmlDocument, "Save and continue");
        return htmlDocument;
    }

    public static IHtmlDocument HasSubmitButton(
        this IHtmlDocument htmlDocument,
        string? text = null)
    {
        GetSubmitButton(htmlDocument, text);

        return htmlDocument;
    }

    public static IHtmlButtonElement GetSubmitButton(
        this IHtmlDocument htmlDocument,
        string? text = null)
    {
        var buttons = HasButton(htmlDocument, "button.govuk-button");

        buttons = WithText(buttons, text);
        buttons = WithAttribute(buttons, "type", "submit");

        return IsSingleHtmlButtonElement(buttons);
    }

    public static IHtmlDocument HasStartButton(
        this IHtmlDocument htmlDocument,
        string? text = null)
    {
        GetStartButton(htmlDocument, text);

        return htmlDocument;
    }

    public static IHtmlButtonElement GetStartButton(
        this IHtmlDocument htmlDocument,
        string? text = null)
    {
        var buttons = HasButton(htmlDocument, "button.govuk-button");

        buttons = WithText(buttons, text);
        buttons = WithClass(buttons, "govuk-button--start");
        buttons = WithAttribute(buttons, "type", "submit");

        return IsSingleHtmlButtonElement(buttons);
    }

    public static IHtmlDocument HasLinkButton(
        this IHtmlDocument htmlDocument,
        string? text = null,
        string? href = null)
    {
        GetLinkButton(htmlDocument, text, href);

        return htmlDocument;
    }

    public static IHtmlAnchorElement GetLinkButton(
        this IHtmlDocument htmlDocument,
        string? text = null,
        string? href = null)
    {
        var buttons = HasButton(htmlDocument, "a.govuk-button");

        buttons = WithText(buttons, text);
        buttons = WithAttribute(buttons, "href", href);

        return IsSingleHtmlAnchorElement(buttons);
    }

    public static IHtmlDocument HasNoButton(this IHtmlDocument htmlDocument, string? text = null)
    {
        var buttons = htmlDocument
            .QuerySelectorAll(".govuk-button")
            .Where(i => string.IsNullOrWhiteSpace(text) || i.TextContent.Contains(text))
            .ToList();

        buttons.Any().Should().BeFalse("Button should not exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkButtonForTestId(this IHtmlDocument htmlDocument, string dataTestId, out IHtmlAnchorElement htmlElement)
    {
        htmlElement = htmlDocument.GetLinkButtonByTestId(dataTestId);

        return htmlDocument;
    }

    private static IList<IElement> HasButton(this IHtmlDocument htmlDocument, string selectors)
    {
        var buttons = htmlDocument
            .QuerySelectorAll(selectors)
            .Select(i => i)
            .ToList();

        buttons.Any().Should().BeTrue("There is no Button on page");

        return buttons;
    }

    private static List<IElement> WithText(IList<IElement> elements, string? text = null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            elements = elements
                .Where(i => i.TextContent.Contains(text))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with text: '{text}'");
        }

        return elements.ToList();
    }

    private static List<IElement> WithClass(IList<IElement> elements, string? className = null)
    {
        if (!string.IsNullOrEmpty(className))
        {
            elements = elements
                .Where(i => i.ClassList.Contains(className))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with class: '{className}'");
        }

        return elements.ToList();
    }

    private static List<IElement> WithAttribute(IList<IElement> elements, string? attributeName = null, string? value = null)
    {
        if (!string.IsNullOrEmpty(value))
        {
            elements = elements
                .Where(i => i.Attributes.Any(a => a.Name == attributeName && a.Value.Contains(value)))
                .ToList();

            elements
                .Any()
                .Should()
                .BeTrue($"There is no Element with attribute : '{attributeName}' value: '{value}'");
        }

        return elements.ToList();
    }

    private static IHtmlButtonElement IsSingleHtmlButtonElement(IList<IElement> elements)
    {
        var buttonElement = elements.SingleOrDefault() as IHtmlButtonElement;
        buttonElement.Should().NotBeNull("There is no single HtmlButtonElement");

        return buttonElement!;
    }

    private static IHtmlAnchorElement IsSingleHtmlAnchorElement(IList<IElement> elements)
    {
        var buttonElement = elements.SingleOrDefault() as IHtmlAnchorElement;
        buttonElement.Should().NotBeNull("There is no single HtmlAnchorElement");

        return buttonElement!;
    }
}
