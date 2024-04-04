using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using HE.Investments.Common.Contract;

namespace HE.Investments.Common.Extensions;

/// <summary>
/// Extensions for common Enum operations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get the 'Display' attribute value for an enum.
    /// </summary>
    /// <param name="enumValue">the enum value.</param>
    /// <returns>the 'Display' attribute string or an empty string if it is not present.</returns>
    public static string GetDisplay(this Enum enumValue)
    {
        return enumValue.GetType().GetField(enumValue.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? string.Empty;
    }

    public static string GetDescription(this Enum enumValue)
    {
        return enumValue.GetType().GetField(enumValue.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
    }

    public static bool IsIn(this Enum? value, params Enum[] values)
    {
        if (value is null)
        {
            return false;
        }

        return values.Contains(value);
    }

    public static bool IsNotIn(this Enum value, params Enum[] values)
    {
        return !values.Contains(value);
    }

    public static T NotDefault<T>(this T enumValue)
        where T : struct, Enum
    {
        if (Convert.ToInt32(enumValue, CultureInfo.InvariantCulture) == 0)
        {
            throw new ArgumentException($"The value of {nameof(enumValue)} cannot be 0.");
        }

        return enumValue;
    }

    public static TEnum GetValueOrFirstValue<TEnum>(this TEnum? enumValue)
        where TEnum : struct, Enum
    {
        return enumValue ?? GetDefinedValues<TEnum>().First();
    }

    public static IEnumerable<TEnum> GetDefinedValues<TEnum>()
        where TEnum : struct, Enum
    {
        return ((TEnum[])Enum.GetValues(typeof(TEnum))).Where(x => Convert.ToInt32(x, CultureInfo.InvariantCulture) != 0);
    }
}
