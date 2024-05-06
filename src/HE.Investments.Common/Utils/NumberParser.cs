using System.Globalization;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.Utils;

public static class NumberParser
{
    private const char DecimalSeparator = '.';

    public static NumberParseResult TryParseDecimal(string? value, decimal minValue, decimal maxValue, int precision, out decimal? result)
    {
        result = null;
        if (value.IsNotProvided())
        {
            return NumberParseResult.ValueMissing;
        }

        value = value!.Trim();
        var isNegative = value.StartsWith('-');
        value = value.StartsWith('-') || value.StartsWith('+') ? value.Remove(0, 1) : value;

        var parts = value.Split(DecimalSeparator);
        if (parts.Length is < 1 or > 2
            || parts[0] == string.Empty
            || parts[0].Any(x => !char.IsDigit(x))
            || (parts.Length > 1 && parts[1].Any(x => !char.IsDigit(x))))
        {
            return NumberParseResult.ValueNotANumber;
        }

        if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            return isNegative ? NumberParseResult.ValueTooLow : NumberParseResult.ValueTooHigh;
        }

        if (decimal.Round(parsedValue, precision) != parsedValue)
        {
            return NumberParseResult.ValueInvalidPrecision;
        }

        result = isNegative ? -parsedValue : parsedValue;
        if (result > maxValue)
        {
            return NumberParseResult.ValueTooHigh;
        }

        if (result < minValue)
        {
            return NumberParseResult.ValueTooLow;
        }

        return NumberParseResult.SuccessfullyParsed;
    }
}
