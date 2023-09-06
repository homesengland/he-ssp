using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;

public static class HtmlDocumentExtensions
{
    public static IHtmlAnchorElement GetAnchorElementById(this IHtmlDocument htmlDocument, string id)
    {
        var elementById = htmlDocument.GetElementById(id);
        elementById.Should().NotBeNull($"Element with Id {id} should exist");

        var anchorElement = elementById as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with Id {id} should be HtmlAnchorElement");

        return anchorElement!;
    }
}
