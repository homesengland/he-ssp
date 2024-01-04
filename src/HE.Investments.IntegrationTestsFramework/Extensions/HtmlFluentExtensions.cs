using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.IntegrationTestsFramework.Assertions;

namespace HE.Investments.IntegrationTestsFramework.Extensions;

public static class HtmlFluentExtensions
{
    public static IHtmlDocument UrlEndWith(this IHtmlDocument htmlDocument, string endsWith)
    {
        htmlDocument.Url.Should().EndWith(endsWith);
        return htmlDocument;
    }

    public static IHtmlDocument UrlWithoutQueryEndsWith(this IHtmlDocument htmlDocument, string endsWith)
    {
        var url = new Uri(htmlDocument.Url);

        url.AbsolutePath.Should().EndWith(endsWith);

        return htmlDocument;
    }

    public static IHtmlDocument HasTitle(this IHtmlDocument htmlDocument, string title)
    {
        htmlDocument.GetPageTitle().Should().Be(title);
        return htmlDocument;
    }

    public static IHtmlDocument HasNotEmptyTitle(this IHtmlDocument htmlDocument)
    {
        htmlDocument.GetPageTitle().Should().NotBeEmpty();
        return htmlDocument;
    }

    public static IHtmlDocument HasLabelTitle(this IHtmlDocument htmlDocument, string titleLabel)
    {
        htmlDocument.GetLabel().Should().Be(titleLabel);
        return htmlDocument;
    }

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

    public static IHtmlDocument HasAnchor(this IHtmlDocument htmlDocument, string elementId, out IHtmlAnchorElement htmlAnchorElement)
    {
        htmlAnchorElement = htmlDocument.GetAnchorElementById(elementId);
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsButton(this IHtmlDocument htmlDocument, string elementId, out IHtmlButtonElement htmlButtonElement)
    {
        var htmlElement = htmlDocument.GetElementById(elementId);
        htmlElement.Should().BeGdsButton();
        htmlButtonElement = (IHtmlButtonElement)htmlElement!;
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsContinueButton(this IHtmlDocument htmlDocument)
    {
        var button = htmlDocument.GetElementById("continue-button");
        button.Should().NotBeNull();
        button.Should().BeGdsButton();
        button!.Text().Trim().Should().Be("Continue");
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsBackButton(this IHtmlDocument htmlDocument)
    {
        var backButton = htmlDocument.GetElementsByClassName("govuk-back-link").SingleOrDefault();
        backButton.Should().NotBeNull();
        backButton!.IsLink().Should().BeTrue();
        backButton!.Text().Trim().Should().Be("Back");
        return htmlDocument;
    }

    public static IHtmlDocument HasGdsInput(this IHtmlDocument htmlDocument, string fieldName)
    {
        var gdsInput = htmlDocument.GetElementsByName(fieldName).SingleOrDefault();
        gdsInput.Should().NotBeNull($"GDS input for field {fieldName} should exist");
        gdsInput!.ClassName.Should().Contain("govuk-input");
        return htmlDocument;
    }

    public static IHtmlDocument DoesNotHaveGdsButton(this IHtmlDocument htmlDocument, string elementId)
    {
        htmlDocument.GetElementById(elementId).Should().BeNull();
        return htmlDocument;
    }

    public static IHtmlDocument HasOneValidationMessages(this IHtmlDocument htmlDocument, string validationMessage)
    {
        return htmlDocument.ContainsOnlyOneValidationMessage(validationMessage);
    }

    public static IHtmlDocument HasValidationMessages(this IHtmlDocument htmlDocument, params string[] validationMessages)
    {
        foreach (var validationMessage in validationMessages)
        {
            htmlDocument.ContainsValidationMessage(validationMessage);
        }

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkButtonForTestId(this IHtmlDocument htmlDocument, string dataTestId, out IHtmlAnchorElement htmlElement)
    {
        htmlElement = htmlDocument.GetLinkButtonByTestId(dataTestId);

        return htmlDocument;
    }

    public static IHtmlDocument HasLinkWithId(this IHtmlDocument htmlDocument, string id, out IHtmlAnchorElement htmlElement)
    {
        var element = htmlDocument.GetElementById(id);

        var anchorElement = element as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with id {id} should be IHtmlAnchorElement");
        htmlElement = anchorElement!;

        return htmlDocument;
    }

    public static IHtmlDocument HasElementForTestId(this IHtmlDocument htmlDocument, string dataTestId, out IElement htmlElement)
    {
        htmlElement = htmlDocument.GetElementByTestId(dataTestId);

        return htmlDocument;
    }

    public static IHtmlDocument HasSuccessNotificationBanner(this IHtmlDocument htmlDocument, string bodyText)
    {
        htmlDocument.GetSuccessNotificationBannerBody().Should().Contain(bodyText);
        return htmlDocument;
    }

    public static IHtmlDocument HasInsetText(this IHtmlDocument htmlDocument, string title)
    {
        htmlDocument.GetInsetText().Should().Be(title);
        return htmlDocument;
    }
}
