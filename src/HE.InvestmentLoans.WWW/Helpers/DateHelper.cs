using System.Globalization;

namespace HE.InvestmentLoans.WWW.Helpers
{
    public static class DateHelper
    {
        public static string ConvertToDateStringWithDescription(string day, string month, string year, string additionalInput)
        {
            if (additionalInput == "Yes")
            {
                string formattedDate = ConvertToDateString(day, month, year);
                return $"{additionalInput} {formattedDate}";
            }

            return additionalInput == "No" ? additionalInput : null;
        }

        public static string ConvertToDateString(string day, string month, string year)
        {
            if (int.TryParse(day, out int dayValue)
                && int.TryParse(month, out int monthValue)
                && int.TryParse(year, out int yearValue))
            {
                DateTime date = new DateTime(yearValue, monthValue, dayValue);
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }
    }
}
