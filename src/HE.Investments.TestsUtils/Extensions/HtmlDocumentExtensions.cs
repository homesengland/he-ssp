using System.Text;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.TestsUtils.Extensions;

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

    public static IElement GetElementByTestId(this IHtmlDocument htmlDocument, string testId)
    {
        var elements = htmlDocument.QuerySelectorAll($"[data-testid='{testId}']");
        elements.Should().NotBeNull($"Element with data-testid {testId} should exist");
        elements.Length.Should().Be(1, $"Only one element with data-testid {testId} should exist");
        return elements.First();
    }

    public static IHtmlAnchorElement GetLinkButtonByTestId(this IHtmlDocument htmlDocument, string testId)
    {
        var element = GetElementByTestId(htmlDocument, testId);

        var anchorElement = element as IHtmlAnchorElement;
        anchorElement.Should().NotBeNull($"Element with data-testid {testId} should be IHtmlAnchorElement");

        if (!anchorElement!.ClassList.Contains("govuk-button"))
        {
            anchorElement.GetElementsByClassName("govuk-button")
                .SingleOrDefault()
                .Should()
                .NotBeNull($"Element with Id {testId} should be HtmlAnchorElement with GdsButton as child");
        }

        return anchorElement;
    }

    public static string GetPageTitle(this IHtmlDocument htmlDocument)
    {
        var header = htmlDocument.GetElementsByClassName(CssConstants.GovUkHxl).FirstOrDefault()
                     ?? htmlDocument.GetElementsByClassName(CssConstants.GovUkHl).FirstOrDefault()
                     ?? htmlDocument.GetElementsByClassName(CssConstants.GovUkFieldSetHeading).FirstOrDefault();

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
        var fieldValidationElements = htmlDocument
            .GetElementsByClassName(CssConstants.GovUkFormGroupError)
            .SelectMany(e => e.GetElementsByClassName(CssConstants.GovUkErrorMessage));

        var fieldValidationErrors = fieldValidationElements!
            .Select(x => x.TextContent.Replace("Error:", string.Empty).Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        fieldValidationErrors.Should().NotBeEmpty();

        return fieldValidationErrors;
    }

    public static IHtmlDocument ContainsOnlyOneValidationMessage(this IHtmlDocument htmlDocument, string errorMessage)
    {
        var pageSummaryErrors = htmlDocument.GetSummaryErrors();
        pageSummaryErrors.Should().OnlyContain(x => x.Equals(errorMessage, StringComparison.Ordinal));

        var fieldValidationErrors = htmlDocument.GetFieldValidationErrors();
        fieldValidationErrors.Should().OnlyContain(x => x.Equals(errorMessage, StringComparison.Ordinal));

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

    public static IHtmlDocument ContainsValidationMessages(this IHtmlDocument htmlDocument, params string[] errorMessages)
    {
        foreach (var errorMessage in errorMessages)
        {
            ContainsValidationMessage(htmlDocument, errorMessage);
        }

        return htmlDocument;
    }

    public static IDictionary<string, string> GetSummaryListItems(this IHtmlDocument htmlDocument)
    {
        var summaryRows = htmlDocument.GetElementsByClassName("govuk-summary-list__row");
        var dictionary = new Dictionary<string, string>();
        foreach (var summaryRow in summaryRows)
        {
            var key = summaryRow.GetElementsByClassName("govuk-summary-list__key").Single().InnerHtml.Trim();
            var value = GetValueFor(summaryRow);

            if (dictionary.ContainsKey(key))
            {
                var summaryHeader = summaryRow.Parent?.PreviousSibling?.PreviousSibling?.Text().Trim() ?? string.Empty;
                dictionary[$"{summaryHeader} - {key}"] = value;
            }
            else
            {
                dictionary[key] = value;
            }
        }

        return dictionary;
    }

    public static string NotificationMessage(this IHtmlDocument htmlDocument)
    {
        var notificationTitle = htmlDocument.GetElementsByClassName("govuk-notification-banner__heading").SingleOrDefault();

        notificationTitle.Should().NotBeNull("Cannot find notification banner at a page.");

        return notificationTitle!.TextContent.Trim();
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

    public static bool ProjectExistsAtTaskList(this IHtmlDocument htmlDocument, string id)
    {
        var projects = htmlDocument.GetTaskListProjects();

        return projects.ContainsKey(id);
    }

    public static (string Name, string Status, string RemoveUrl) GetProjectFromTaskList(this IHtmlDocument htmlDocument, string id)
    {
        var projects = htmlDocument.GetTaskListProjects();

        if (!projects.ContainsKey(id))
        {
            throw new ArgumentException($"Cannot find project id: {id} in task list. Available projects: {string.Join(", ", projects.Keys)}.");
        }

        return htmlDocument.GetTaskListProjects()[id];
    }

    public static IDictionary<string, (string Name, string Status, string RemoveUrl)> GetTaskListProjects(this IHtmlDocument htmlDocument)
    {
        var projectRows = htmlDocument.GetElementsByClassName("task-list-grid-container");

        var dictionary = new Dictionary<string, (string Name, string Status, string RemoveUrl)>();
        foreach (var row in projectRows)
        {
            var projectLink = row
                .GetElementsByClassName("task-list-project-name")
                .First()
                .GetElementsByTagName("a")
                .First();

            var id = projectLink.GetAttribute("href")!.GetProjectGuidFromRelativePath();

            var name = projectLink.InnerHtml.Trim();

            var status = row.GetElementsByClassName("app-task-list__tag").FirstOrDefault()?.InnerHtml.Trim() ?? string.Empty;

            var removeUrl = row.GetElementsByClassName("task-list-remove-link").First().GetElementsByTagName("a").First().GetAttribute("href")!;

            dictionary[id] = (name, status, removeUrl);
        }

        return dictionary;
    }

    public static string GetSuccessNotificationBannerBody(this IHtmlDocument htmlDocument)
    {
        var successNotificationBanner = htmlDocument.GetElementsByClassName(CssConstants.GovUkNotificationBannerSuccess).FirstOrDefault();
        successNotificationBanner.Should().NotBeNull("Success notification banner does not exist");

        var notificationBannerContent = successNotificationBanner?.GetElementsByClassName(CssConstants.GovUkNotificationBannerContent).FirstOrDefault();
        notificationBannerContent.Should().NotBeNull("Notification banner does not have content");

        return notificationBannerContent!.TextContent.Trim();
    }

    public static string GetInsetText(this IHtmlDocument htmlDocument)
    {
        var insetText = htmlDocument.GetElementsByClassName(CssConstants.GovUkInsetText).FirstOrDefault();
        insetText.Should().NotBeNull("Inset text does not exist");

        return insetText!.InnerHtml.Trim();
    }

    private static string GetValueFor(IElement summaryRow)
    {
        var valueRow = summaryRow.GetElementsByClassName("govuk-summary-list__value").Single();

        var valueBuilder = new StringBuilder();
        if (valueRow.Children.Length > 1)
        {
            foreach (var child in valueRow.Children)
            {
                valueBuilder.AppendLine(child.TextContent.Trim());
            }
        }
        else if (valueRow.Children.Length == 1)
        {
            valueBuilder.Append(valueRow.LastElementChild!.InnerHtml.Trim());
        }
        else
        {
            return string.Empty;
        }

        var value = valueBuilder.ToString();
        return value;
    }
}
