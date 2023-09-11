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

    public static IHtmlDocument HasTitle(this IHtmlDocument htmlDocument, string title)
    {
        htmlDocument.GetPageTitle().Should().Be(title);
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

    public static IHtmlDocument HasGdsButton(this IHtmlDocument htmlDocument, string elementId)
    {
        htmlDocument.GetElementById(elementId).Should().BeGdsButton();
        return htmlDocument;
    }

    public static IHtmlDocument HasValidationMessages(this IHtmlDocument htmlDocument, string validationMessage)
    {
        return htmlDocument.ContainsValidationMessage(validationMessage);
    }
}
