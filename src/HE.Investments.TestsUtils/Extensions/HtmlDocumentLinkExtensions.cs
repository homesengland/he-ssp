using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlDocumentLinkExtensions
{
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

    public static IHtmlDocument HasSaveAndReturnToApplicationLinkButton(this IHtmlDocument htmlDocument)
    {
        htmlDocument
            .HasElementWithText("button", "Save and return to application");

        return htmlDocument;
    }
}
