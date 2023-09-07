using System.Globalization;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Assertions;

namespace HE.InvestmentLoans.IntegrationTests.LoansHelpers.Extensions;

public static class TaskListExtensions
{
    public static DateTime ExtractLastSavedDateFromTaskListPage(this IHtmlDocument taskListPage)
    {
        var lastSavedLabel = taskListPage.GetElementById("last-saved-label");
        lastSavedLabel.Should().NotBeNull("Last saved data should be visible");

        var dateTimeAsString = lastSavedLabel!.TextContent.Trim().Replace("Last saved on ", string.Empty).Replace("at ", string.Empty);
        var dateTime = DateTime.Parse(dateTimeAsString, CultureInfo.InvariantCulture);
        return dateTime;
    }
}
