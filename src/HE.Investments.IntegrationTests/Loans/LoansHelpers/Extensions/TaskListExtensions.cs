using System.Globalization;
using AngleSharp.Html.Dom;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Assertions;

namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;

public static class TaskListExtensions
{
    public static IHtmlDocument ExtractLastSavedDateFromTaskListPage(this IHtmlDocument taskListPage, out DateTime dateTime)
    {
        var lastSavedLabel = taskListPage.GetElementById("last-saved-label");
        lastSavedLabel.Should().NotBeNull("Last saved data should be visible");

        var dateTimeAsString = lastSavedLabel!.TextContent.Replace("Last saved on ", string.Empty).Split("by").First().Trim();
        dateTime = DateTime.Parse(dateTimeAsString, CultureInfo.GetCultureInfo("en-GB"));
        return taskListPage;
    }

    public static IHtmlDocument ExtractSubmittedOnDateFromTaskListPage(this IHtmlDocument taskListPage, out DateTime dateTime)
    {
        var submittedOnLabel = taskListPage.GetElementById("submitted-on-label");
        submittedOnLabel.Should().NotBeNull("Submitted on data should be visible");

        var dateTimeAsString = submittedOnLabel!.TextContent.Replace("Submitted on ", string.Empty).Trim();
        dateTime = DateTime.Parse(dateTimeAsString, CultureInfo.GetCultureInfo("en-GB"));
        return taskListPage;
    }
}
