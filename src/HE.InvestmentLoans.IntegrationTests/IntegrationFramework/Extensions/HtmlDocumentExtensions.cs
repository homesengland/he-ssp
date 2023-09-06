using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;

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

    public static IHtmlButtonElement GetGdsSubmitButtonById(this IHtmlDocument htmlDocument, string id)
    {
        var elementById = htmlDocument.GetElementById(id);
        elementById.Should().NotBeNull($"Element with Id {id} should exist");

        var buttonElement = elementById as IHtmlButtonElement;
        buttonElement.Should().NotBeNull($"Element with Id {id} should be HtmlButtonElement");

        buttonElement!.ClassName.Should().Contain("govuk-button", $"Element with Id {id} should be HtmlButtonElement with govuk-button class name");
        buttonElement.Form.Should().NotBeNull("Form is required to perform submit");

        return buttonElement;
    }

    public static IHtmlAnchorElement GetGdsLinkButtonById(this IHtmlDocument htmlDocument, string id)
    {
        var elementById = htmlDocument.GetElementById(id);
        elementById.Should().NotBeNull($"Element with Id {id} should exist");

        var anchorElement = elementById as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with Id {id} should be HtmlAnchorElement with GdsButton as child");

        anchorElement!.GetElementsByClassName("govuk-button").SingleOrDefault().Should().NotBeNull($"Element with Id {id} should be HtmlAnchorElement with GdsButton as child");
        return anchorElement;
    }

    public static string GetPageTitle(this IHtmlDocument htmlDocument)
    {
        var header = htmlDocument.GetElementsByClassName(CssConstants.GovUkHxl).FirstOrDefault();
        header.Should().NotBeNull("Page Header does not exist");
        return header!.InnerHtml.Trim();
    }
}
