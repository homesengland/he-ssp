using System.Globalization;

namespace HE.InvestmentLoans.WWW.Helpers
{
    public static class DateHelper
    {
        public static string ConvertToDateStringWithDescription(string day, string month, string year, string additionalInput)
        {
            if (additionalInput == "Yes"
                && int.TryParse(day, out int dayValue)
                && int.TryParse(month, out int monthValue)
                && int.TryParse(year, out int yearValue))
            {
                DateTime date = new DateTime(yearValue, monthValue, dayValue);
                string formattedDate = date.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                return $"{additionalInput}, {formattedDate}";
            }

            return additionalInput == "No" ? additionalInput : null;
        }
    }
}
