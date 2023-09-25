using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;

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
        var header = htmlDocument.GetElementsByClassName(CssConstants.GovUkHxl).FirstOrDefault()
                     ?? htmlDocument.GetElementsByClassName(CssConstants.GovUkFieldSetHeading).FirstOrDefault()
                     ?? htmlDocument.GetElementsByClassName(CssConstants.GovUkHl).FirstOrDefault();

        header.Should().NotBeNull("Page Header does not exist");
        return header!.InnerHtml.Trim();
    }

    public static string GetLabel(this IHtmlDocument htmlDocument)
    {
        var label = htmlDocument.GetElementsByClassName(CssConstants.GovUkLabel).FirstOrDefault();

        label.Should().NotBeNull("Page label does not exist");
        return label!.InnerHtml.Trim();
    }

    public static string[] GetSummaryErrors(this IHtmlDocument htmlDocument)
    {
        var errorSummary = htmlDocument.GetElementsByClassName(CssConstants.GovUkErrorSummary).SingleOrDefault();
        errorSummary.Should().NotBeNull("Error summary should be present on a page");
        var errorItems = errorSummary!.GetElementsByTagName("a").Select(x => x.TextContent.Trim()).ToArray();
        errorItems.Should().NotBeEmpty();
        return errorItems;
    }

    public static string[] GetFieldValidationErrors(this IHtmlDocument htmlDocument)
    {
        var fieldValidationError = htmlDocument.GetElementsByClassName(CssConstants.GovUkErrorMessage).ToArray();
        var errorItems = fieldValidationError!
                                .Select(x => x.TextContent.Replace("Error:", string.Empty).Trim())
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToArray();
        errorItems.Should().HaveCountGreaterThan(1, "Field validation error should be present on a page");

        return errorItems;
    }

    public static IHtmlDocument ContainsOnlyOneValidationMessage(this IHtmlDocument htmlDocument, string errorMessage)
    {
        var pageSummaryErrors = htmlDocument.GetSummaryErrors();
        var fieldValidationErrors = htmlDocument.GetFieldValidationErrors();

        pageSummaryErrors.Should().OnlyContain(x => x.Equals(errorMessage, StringComparison.Ordinal));
        fieldValidationErrors.Should().OnlyContain(x => x.Equals(errorMessage, StringComparison.Ordinal));
        htmlDocument.GetElementsByClassName(CssConstants.GovUkFormGroupError).Should().NotBeNull("Error message for specific item should exist");
        return htmlDocument;
    }

    public static IHtmlDocument ContainsValidationMessage(this IHtmlDocument htmlDocument, string errorMessage)
    {
        var pageErrors = htmlDocument.GetSummaryErrors();
        var fieldValidationErrors = htmlDocument.GetFieldValidationErrors();

        pageErrors.Should().Contain(x => x.Equals(errorMessage, StringComparison.Ordinal));
        fieldValidationErrors.Should().Contain(x => x.Equals(errorMessage, StringComparison.Ordinal));
        htmlDocument.GetElementsByClassName(CssConstants.GovUkFormGroupError).Should().NotBeNull("Error message for specific item should exist");
        return htmlDocument;
    }

    public static IDictionary<string, string> GetSummaryListItems(this IHtmlDocument htmlDocument)
    {
        var summaryRows = htmlDocument.GetElementsByClassName("govuk-summary-list__row");
        var dictionary = new Dictionary<string, string>();
        foreach (var summaryRow in summaryRows)
        {
            var key = summaryRow.GetElementsByClassName("govuk-summary-list__key").Single().InnerHtml.Trim();
            var value = summaryRow.GetElementsByClassName("govuk-summary-list__value").Single().LastElementChild!.InnerHtml.Trim();
            dictionary[key] = value;
        }

        return dictionary;
    }

    public static IDictionary<string, string> GetTaskListItems(this IHtmlDocument htmlDocument)
    {
        var summaryRows = htmlDocument.GetElementsByClassName("app-task-list__item");
        var dictionary = new Dictionary<string, string>();
        foreach (var summaryRow in summaryRows)
        {
            var key = summaryRow.GetElementsByClassName("app-task-list__task-name").First().TextContent.Trim();
            var value = summaryRow.GetElementsByClassName("app-task-list__tag").FirstOrDefault()?.InnerHtml.Trim() ?? string.Empty;
            dictionary[key] = value;
        }

        return dictionary;
    }

    public static IDictionary<string, (string name, string status)> GetTaskListProjects(this IHtmlDocument htmlDocument)
    {
        var projectRows = htmlDocument.GetElementsByClassName("task-list-grid-container");

        var dictionary = new Dictionary<string, (string name, string status)>();
        foreach (var row in projectRows)
        {
            var key = row
                .GetElementsByClassName("task-list-project-name").First()
                .GetElementsByTagName("a").First()
                .GetAttribute("href")!.GetProjectGuidFromUrl();

            var status = row.GetElementsByClassName("app-task-list__tag").FirstOrDefault()?.InnerHtml.Trim() ?? string.Empty;
            //dictionary[key] = value;
        }

        return dictionary;
    }
}
