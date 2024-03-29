using System.Globalization;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.WWW.Helpers;

public static class DateHelper
{
    public static string? ConvertToDateStringWithDescription(string day, string month, string year, string additionalInput)
    {
        if (additionalInput == "Yes")
        {
            var formattedDate = ConvertToDateString(day, month, year);
            return $"{additionalInput}, {formattedDate}";
        }

        return additionalInput == "No" ? additionalInput : null;
    }

    public static string ConvertToDateString(string day, string month, string year)
    {
        if (int.TryParse(day, out var dayValue)
            && int.TryParse(month, out var monthValue)
            && int.TryParse(year, out var yearValue))
        {
            var date = new DateTime(yearValue, monthValue, dayValue, 0, 0, 0, DateTimeKind.Unspecified);
            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        return string.Empty;
    }

    public static string? DisplayAsUkFormatDate(DateDetails? utcDateTime)
    {
        if (utcDateTime == null)
        {
            return null;
        }

        if (DateTime.TryParseExact(
                $"{utcDateTime.Day}/{utcDateTime.Month}/{utcDateTime.Year}",
                "d/M/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var dateOnly))
        {
            return DisplayAsUkFormatOnlyDate(dateOnly);
        }

        return null;
    }

    public static string? DisplayAsUkFormatOnlyMonthAndYearDate(DateDetails? utcDateTime)
    {
        if (utcDateTime == null)
        {
            return null;
        }

        if (DateTime.TryParseExact(
                $"{utcDateTime.Month}/{utcDateTime.Year}",
                "M/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out var dateOnly))
        {
            return GetOnlyMonthAndYearAsUkFormat(dateOnly);
        }

        return null;
    }

    public static string? DisplayAsUkFormatOnlyDate(DateTime? utcDateTime)
    {
        if (!utcDateTime.HasValue)
        {
            return null;
        }

        return DateOnly.FromDateTime(utcDateTime.Value.ConvertUtcToUkLocalTime()).ToString(Culture.Uk);
    }

    public static string? DisplayAsUkFormatDateTime(DateTime? utcDateTime)
    {
        return utcDateTime?.ConvertUtcToUkLocalTime().ToString(Culture.Uk);
    }

    private static string? GetOnlyMonthAndYearAsUkFormat(DateTime? utcDateTime)
    {
        var onlyDateUkFormat = DisplayAsUkFormatOnlyDate(utcDateTime);
        return onlyDateUkFormat?[3..];
    }
}
