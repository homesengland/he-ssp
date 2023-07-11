using System.Globalization;

namespace HE.InvestmentLoans.WWW.Helpers;

public static class DateHelper
{
    public static string ConvertToDateStringWithDescription(string day, string month, string year, string additionalInput)
    {
        if (additionalInput == "Yes")
        {
            var formattedDate = ConvertToDateString(day, month, year);
            return $"{additionalInput} {formattedDate}";
        }

        return additionalInput == "No" ? additionalInput : null;
    }

    public static string ConvertToDateString(string day, string month, string year)
    {
        if (int.TryParse(day, out var dayValue)
            && int.TryParse(month, out var monthValue)
            && int.TryParse(year, out var yearValue))
        {
            var date = new DateTime(yearValue, monthValue, dayValue);
            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        return string.Empty;
    }
}
