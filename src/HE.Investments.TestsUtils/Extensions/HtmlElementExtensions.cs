using System.Globalization;
using System.Text;
using System.Web;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Helpers;

namespace HE.Investments.TestsUtils.Extensions;

public static class HtmlElementExtensions
{
    public static IDictionary<string, SummaryItem> GetSummaryListItems(this IElement summary)
    {
        var summaryRows = summary.GetElementsByClassName("govuk-summary-list__row");
        var dictionary = new Dictionary<string, SummaryItem>();
        var duplicateKeyCounter = 1;
        foreach (var summaryRow in summaryRows)
        {
            var key = summaryRow.GetElementsByClassName("govuk-summary-list__key").Single().InnerHtml.Trim();
            var header = summaryRow.Parent?.PreviousSibling?.PreviousSibling?.Text().Trim() ?? string.Empty;
            var value = GetValueFor(summaryRow);
            var link = summaryRow.QuerySelector<IHtmlAnchorElement>(".govuk-summary-list__actions a");
            var item = new SummaryItem(header, key, value, link);

            if (!dictionary.TryAdd(key, item))
            {
                dictionary[$"{(header.IsProvided() ? header : duplicateKeyCounter.ToString(CultureInfo.InvariantCulture))} - {key}"] = item;
                duplicateKeyCounter++;
            }
        }

        return dictionary;
    }

    public static IElement HasLinkWithText(this IElement htmlElement, string text, out IHtmlAnchorElement matchedLink)
    {
        matchedLink = HtmlElementFilters.WithText(htmlElement.GetLinks(), text).SingleOrDefault()!;
        matchedLink.Should().NotBeNull("There is no single Link element on page");

        return htmlElement;
    }

    private static List<IHtmlAnchorElement> GetLinks(this IElement htmlElement)
    {
        return htmlElement
            .QuerySelectorAll("a.govuk-link")
            .OfType<IHtmlAnchorElement>()
            .ToList();
    }

    private static string GetValueFor(IElement summaryRow)
    {
        var valueRow = summaryRow.GetElementsByClassName("govuk-summary-list__value").SingleOrDefault();

        var valueBuilder = new StringBuilder();
        if (valueRow?.Children.Length > 1)
        {
            foreach (var child in valueRow.Children)
            {
                valueBuilder.AppendLine(child.TextContent.Trim());
            }
        }
        else if (valueRow?.Children.Length == 1)
        {
            valueBuilder.Append(valueRow.LastElementChild!.InnerHtml.Trim());
        }
        else if (!string.IsNullOrEmpty(valueRow?.InnerHtml))
        {
            valueBuilder.Append(valueRow.InnerHtml);
        }
        else
        {
            return string.Empty;
        }

        var value = valueBuilder.ToString();
        return HttpUtility.HtmlDecode(value);
    }
}
