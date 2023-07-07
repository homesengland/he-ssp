namespace HE.InvestmentLoans.WWW.Extensions
{
    using System.Collections.Generic;
    using System.Text;
    using HE.InvestmentLoans.WWW.Helpers;
    using HE.InvestmentLoans.WWW.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class SelectItemOptionsListExtensions
    {
        public static string GetSummaryLabel(this List<SelectItemWithSummaryLabel> options, IEnumerable<string> values)
        {
            if (values is null)
                return null;

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
            if (values is null || values.Count() == 0)
                return null;

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
                .Where(r => r.Value == value)
                .Select(r => r.Text)
                .FirstOrDefault();
            return result ?? value;
        }
    }
}
