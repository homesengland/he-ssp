using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using ElementExtensions = HE.Investments.TestsUtils.Assertions.ElementExtensions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentButtonExtensions
{
    public static IHtmlDocument HasGdsSubmitButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlButtonElement htmlButtonElement)
    {
        htmlButtonElement = htmlDocument.GetGdsSubmitButtonById(elementId);
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsLinkButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlAnchorElement htmlAnchorElement)
    {
        htmlAnchorElement = htmlDocument.GetGdsLinkButtonById(elementId);
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlButtonElement htmlButtonElement)
    {
        var htmlElement = htmlDocument.GetElementById(elementId);
        ElementExtensions.Should(htmlElement).BeGdsButton();
        htmlButtonElement = (IHtmlButtonElement)htmlElement!;
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsContinueButton(this IHtmlDocument htmlDocument)
    {
        htmlDocument.HasGdsButton("continue-button", out var button);
        button.Text().Trim().Should().Be("Continue");
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsSaveAndContinueButton(this IHtmlDocument htmlDocument)
    {
        htmlDocument.HasGdsButton("continue-button", out var button);
        button.Text().Trim().Should().Be("Save and continue");
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsBackButton(this IHtmlDocument htmlDocument, bool validateLink = true)
    {
        var backButton = htmlDocument.GetElementsByClassName("govuk-back-link").SingleOrDefault();
        backButton.Should().NotBeNull();
        if (validateLink)
        {
            backButton!.IsLink().Should().BeTrue();
        }

        backButton!.Text().Trim().Should().Be("Back");
        return htmlDocument;
    }

    public static IHtmlDocument DoesNotHaveGdsButton(this IHtmlDocument htmlDocument, string elementId)
    {
        htmlDocument.GetElementById(elementId).Should().BeNull();
        return htmlDocument;
    }

    public static IHtmlDocument HasLinkButtonForTestId(this IHtmlDocument htmlDocument, string dataTestId, out IHtmlAnchorElement htmlElement)
    {
        htmlElement = htmlDocument.GetLinkButtonByTestId(dataTestId);

        return htmlDocument;
    }

    public static IHtmlDocument HasSaveAndReturnToApplicationLinkButton(this IHtmlDocument htmlDocument)
    {
        htmlDocument
            .HasElementWithText("button", "Save and return to application");

        return htmlDocument;
    }
}
