using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using ElementExtensions = HE.Investments.TestsUtils.Assertions.ElementExtensions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentButtonLegacyExtensions
{
    public static IHtmlDocument HasGdsSubmitButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlButtonElement htmlButtonElement)
    {
        htmlButtonElement = htmlDocument.GetGdsSubmitButtonById(elementId);
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlButtonElement htmlButtonElement)
    {
        var htmlElement = htmlDocument.GetElementById(elementId);
        ElementExtensions.Should(htmlElement).BeGdsButton();
        htmlButtonElement = (IHtmlButtonElement)htmlElement!;
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsSaveAndContinueButton(this IHtmlDocument htmlDocument)
    {
        htmlDocument.HasGdsButton("continue-button", out var button);
        button.Text().Trim().Should().Be("Save and continue");
        return htmlDocument;
    }
}
