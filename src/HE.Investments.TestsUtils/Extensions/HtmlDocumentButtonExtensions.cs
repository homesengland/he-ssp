using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Extensions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentButtonExtensions
{
    public static IHtmlDocument HasContinueButton(this IHtmlDocument htmlDocument) => htmlDocument.HasContinueButton(out _);

    public static IHtmlDocument HasContinueButton(this IHtmlDocument htmlDocument, out IHtmlButtonElement button)
    {
        button = GetContinueButton(htmlDocument);
        return htmlDocument;
    }

    public static IHtmlButtonElement GetContinueButton(this IHtmlDocument htmlDocument)
    {
        return GetSubmitButton(htmlDocument, "Continue");
    }

    public static IHtmlDocument HasSaveAndContinueButton(this IHtmlDocument htmlDocument) => htmlDocument.HasSaveAndContinueButton(out _);

    public static IHtmlDocument HasSaveAndContinueButton(this IHtmlDocument htmlDocument, out IHtmlButtonElement button)
    {
        button = GetSubmitButton(htmlDocument, "Save and continue");
        return htmlDocument;
    }

    public static IHtmlDocument HasSubmitButton(
        this IHtmlDocument htmlDocument,
        out IHtmlButtonElement button,
        string? text = null)
    {
        button = GetSubmitButton(htmlDocument, text);

        return htmlDocument;
    }

    public static IHtmlButtonElement GetSubmitButton(
        this IHtmlDocument htmlDocument,
        string? text = null)
    {
        var buttons = HasButton(htmlDocument, "button.govuk-button");

        buttons = HtmlElementFilters.WithText(buttons, text);
        buttons = HtmlElementFilters.WithAttribute(buttons, "type", "submit");

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

        buttons = HtmlElementFilters.WithText(buttons, text);
        buttons = HtmlElementFilters.WithClass(buttons, "govuk-button--start");
        buttons = HtmlElementFilters.WithAttribute(buttons, "type", "submit");

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

        buttons = HtmlElementFilters.WithText(buttons, text);
        buttons = HtmlElementFilters.WithAttribute(buttons, "href", href);

        return IsSingleHtmlAnchorElement(buttons);
    }

    public static IHtmlDocument HasNoButton(this IHtmlDocument htmlDocument, string? text = null)
    {
        var buttons = htmlDocument
            .QuerySelectorAll(".govuk-button")
            .Where(i => string.IsNullOrWhiteSpace(text) || i.TextContent.Contains(text))
            .ToList();

        buttons.IsEmpty().Should().BeTrue("Button should not exist");

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkButtonForTestId(this IHtmlDocument htmlDocument, string dataTestId, out IHtmlAnchorElement htmlElement)
    {
        htmlElement = htmlDocument.GetLinkButtonByTestId(dataTestId);

        return htmlDocument;
    }

    private static List<IElement> HasButton(this IHtmlDocument htmlDocument, string selectors)
    {
        var buttons = htmlDocument
            .QuerySelectorAll(selectors)
            .Select(i => i)
            .ToList();

        buttons.IsNotEmpty().Should().BeTrue("There is no Button on page");

        return buttons;
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
