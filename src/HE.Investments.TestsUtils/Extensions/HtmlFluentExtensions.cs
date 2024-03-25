using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlFluentExtensions
{
    public static IHtmlDocument UrlEndWith(this IHtmlDocument htmlDocument, string endsWith)
    {
        htmlDocument.Url.ToLowerInvariant().Should().EndWith(endsWith.ToLowerInvariant());
        return htmlDocument;
    }

    public static IHtmlDocument UrlWithoutQueryEndsWith(this IHtmlDocument htmlDocument, string endsWith)
    {
        var url = new Uri(htmlDocument.Url);

        url.AbsolutePath.ToLowerInvariant().Should().EndWith(endsWith.ToLowerInvariant());

        return htmlDocument;
    }

    public static IHtmlDocument UrlWithoutQueryNotEndsWith(this IHtmlDocument htmlDocument, string endsWith)
    {
        var url = new Uri(htmlDocument.Url);

        url.AbsolutePath.ToLowerInvariant().Should().NotEndWith(endsWith.ToLowerInvariant());

        return htmlDocument;
    }

    public static IHtmlDocument HasTitle(this IHtmlDocument htmlDocument, string title)
    {
        htmlDocument.GetPageTitle().Should().Be(title);
        return htmlDocument;
    }

    public static IHtmlDocument HasMatchingTitle(this IHtmlDocument htmlDocument, string titlePattern)
    {
        var title = htmlDocument.GetPageTitle();
        new Regex(titlePattern).IsMatch(title).Should().BeTrue($"\"{title}\" should match \"{titlePattern}\"");

        return htmlDocument;
    }

    public static IHtmlDocument HasStatusTagByTestId(this IHtmlDocument htmlDocument, string status, string testId)
    {
        htmlDocument.GetStatusTagByTestId(testId).Should().Be(status);
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

    public static IHtmlDocument HasElementForTestId(this IHtmlDocument htmlDocument, string testId, out IElement htmlElement)
    {
        htmlElement = htmlDocument.GetElementByTestId(testId);

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

    public static IHtmlDocument ContainsInsetText(this IHtmlDocument htmlDocument, string title)
    {
        htmlDocument.GetInsetText().Should().Contain(title);
        return htmlDocument;
    }

    public static IHtmlDocument HasSectionWithStatus(this IHtmlDocument htmlDocument, string sectionStatusId, string expectedStatus)
    {
        return htmlDocument.HasElementWithTextByTestId(sectionStatusId, expectedStatus);
    }

    private static IHtmlDocument HasElementWithTextByTestId(this IHtmlDocument htmlDocument, string testId, string text)
    {
        var element = htmlDocument.GetElementByTestId(testId);

        element.Should().NotBeNull($"Element with data-testId {testId} does not exist");
        element.TextContent.Should().Contain(text, $"Element with data-testId {testId} is missing text \"{text}\"");

        return htmlDocument;
    }
}
