namespace HE.InvestmentLoans.Common.Extensions;

public static class EnumExtensions
{
    public static bool IsIn(this Enum value, params Enum[] values)
    {
        return values.Contains(value);
    }

    public static bool IsNotIn(this Enum value, params Enum[] values)
    {
        return !values.Contains(value);
    }
}
