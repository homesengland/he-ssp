using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
        return enumValue?.GetType()?.GetField(enumValue.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? string.Empty;
    }

    public static string GetDescription(this Enum enumValue)
    {
        return enumValue.GetType().GetField(enumValue.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
    }

    public static bool IsIn(this Enum value, params Enum[] values)
    {
        return values.Contains(value);
    }

    public static bool IsNotIn(this Enum value, params Enum[] values)
    {
        return !values.Contains(value);
    }
}
