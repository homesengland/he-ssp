using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.TestsUtils.Extensions;
using ElementExtensions = HE.Investments.TestsUtils.Assertions.ElementExtensions;

namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;

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

    public static IHtmlButtonElement GetGdsSubmitButtonByTestId(this IHtmlDocument htmlDocument, string testId)
    {
        var elementById = htmlDocument.GetElementByTestId(testId);
        elementById.Should().NotBeNull($"Element with test-id {testId} should exist");

        var buttonElement = elementById as IHtmlButtonElement;
        buttonElement.Should().NotBeNull($"Element with test-id {testId} should be HtmlButtonElement");

        buttonElement!.ClassName.Should().Contain("govuk-button", $"Element with test-id {testId} should be HtmlButtonElement with govuk-button class name");
        buttonElement.Form.Should().NotBeNull("Form is required to perform submit");

        return buttonElement;
    }

    public static IHtmlAnchorElement GetGdsLinkButtonById(this IHtmlDocument htmlDocument, string id)
    {
        var elementById = htmlDocument.GetElementById(id);
        elementById.Should().NotBeNull($"Element with Id {id} should exist");

        var anchorElement = elementById as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with Id {id} should be HtmlAnchorElement which contains GdsButton");

        if (!anchorElement!.ClassList.Contains("govuk-button"))
        {
            anchorElement.GetElementsByClassName("govuk-button")
                .SingleOrDefault()
                .Should()
                .NotBeNull($"Element with Id {id} should be HtmlAnchorElement with GdsButton as child");
        }

        return anchorElement;
    }

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

    public static IHtmlDocument HasAnchorElementById(this IHtmlDocument htmlDocument, string elementId, out IHtmlAnchorElement htmlAnchorElement)
    {
        htmlAnchorElement = htmlDocument.GetAnchorElementById(elementId);
        return htmlDocument;
    }
}
