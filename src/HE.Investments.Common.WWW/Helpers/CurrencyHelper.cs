namespace HE.Investments.Common.WWW.Helpers;

public static class CurrencyHelper
{
    private const char PoundsChar = '£';

    [Flags]
    private enum CurrencyDisplaySettings
    {
        None = 0,
        WithoutPoundsSymbol = 1,
        WithoutPences = 2,
        WithoutThousandsSeparator = 4,
    }

    public static string? InputPounds(decimal? value)
    {
        return Format(
            value,
            CurrencyDisplaySettings.WithoutPences | CurrencyDisplaySettings.WithoutPoundsSymbol | CurrencyDisplaySettings.WithoutThousandsSeparator);
    }

    public static string? InputPoundsPences(decimal? value)
    {
        return Format(value, CurrencyDisplaySettings.WithoutPoundsSymbol | CurrencyDisplaySettings.WithoutThousandsSeparator);
    }

    public static string DisplayPounds(decimal value)
    {
        return Format(value, CurrencyDisplaySettings.WithoutPences);
    }

    public static string? DisplayPounds(decimal? value)
    {
        return Format(value, CurrencyDisplaySettings.WithoutPences);
    }

    public static string? DisplayPoundsPences(this decimal? value)
    {
        return Format(value, CurrencyDisplaySettings.None);
    }

    private static string? Format(decimal? value, CurrencyDisplaySettings displaySettings)
    {
        return value.HasValue ? Format(value.Value, displaySettings) : null;
    }

    private static string Format(decimal value, CurrencyDisplaySettings displaySettings)
    {
        var stringValue = RemoveZeroPences(value.ToString(GetNumberFormat(displaySettings), Culture.Uk));

        return displaySettings.HasFlag(CurrencyDisplaySettings.WithoutPoundsSymbol)
            ? stringValue
            : $"{PoundsChar}{stringValue}";
    }

    private static string GetNumberFormat(CurrencyDisplaySettings displaySettings)
    {
        if (displaySettings.HasFlag(CurrencyDisplaySettings.WithoutThousandsSeparator))
        {
            return displaySettings.HasFlag(CurrencyDisplaySettings.WithoutPences) ? "0" : "0.00";
        }

        return displaySettings.HasFlag(CurrencyDisplaySettings.WithoutPences) ? "N0" : "N2";
    }

    private static string RemoveZeroPences(string value)
    {
        return value.Replace(".00", string.Empty);
    }
}
