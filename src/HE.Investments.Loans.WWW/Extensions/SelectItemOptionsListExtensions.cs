using System.Globalization;
using System.Text;
using HE.Investments.Loans.WWW.Helpers;
using HE.Investments.Loans.WWW.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Loans.WWW.Extensions;

public static class SelectItemOptionsListExtensions
{
    public static string GetSummaryLabel(this List<SelectItemWithSummaryLabel> options, IEnumerable<string> values)
    {
        if (values is null)
        {
            return null;
        }

        return values
            .Aggregate(
                new StringBuilder(),
                (current, next) =>
                {
                    if (current.Length != 0)
                    {
                        current.Append(", ");
                    }

                    current.Append(options.GetSummaryLabel(next));

                    return current;
                }).ToString();
    }

    public static string GetSummaryLabel(this List<SelectListItem> options, IEnumerable<string> values)
    {
        if (values is null || !values.Any())
        {
            return null;
        }

        return values
            .Aggregate(
                new StringBuilder(),
                (current, next) =>
                {
                    if (current.Length != 0)
                    {
                        current.Append(", ");
                    }

                    current.Append(options.GetSummaryLabel(next));

                    return current;
                }).ToString();
    }

    public static string GetSummaryLabel(this List<SelectItemWithSummaryLabel> options, string value)
    {
        return options
            .Where(r => r.Value == value)
            .Select(r => r.SummaryLabel)
            .FirstOrDefault();
    }

    public static string GetSummaryLabel(this List<SelectItemWithSummaryLabel> options, string value, string details)
    {
        var label = options
            .Where(r => r.Value == value)
            .Select(r => r.SummaryLabel)
            .FirstOrDefault();

        return LabelHelper.GetSummaryLabel(label, details);
    }

    public static string GetSummaryLabel(this List<SelectListItem> options, string value)
    {
        var result = options
            .Where(r => r.Value?.ToLower(CultureInfo.InvariantCulture) == value?.ToLower(CultureInfo.InvariantCulture))
            .Select(r => r.Text)
            .FirstOrDefault();
        return result ?? value;
    }
}
