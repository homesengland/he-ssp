using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Assertions;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;

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

    public static IHtmlDocument HasGdsButton(this IHtmlDocument htmlDocument, string elementId)
    {
        htmlDocument.GetElementById(elementId).Should().BeGdsButton();
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
}
