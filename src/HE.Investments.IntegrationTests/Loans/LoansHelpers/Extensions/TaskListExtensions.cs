using System.Globalization;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Assertions;

namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Extensions;

public static class TaskListExtensions
{
    public static IHtmlDocument ExtractLastSavedDateFromTaskListPage(this IHtmlDocument taskListPage, out DateTime dateTime)
    {
        var lastSavedLabel = taskListPage.GetElementById("last-saved-label");
        lastSavedLabel.Should().NotBeNull("Last saved data should be visible");

        var dateTimeAsString = lastSavedLabel!.TextContent.Trim().Replace("Last saved on ", string.Empty).Replace("at ", string.Empty);
        dateTime = DateTime.Parse(dateTimeAsString, CultureInfo.InvariantCulture);
        return taskListPage;
    }
}
