namespace HE.Investments.Common.Extensions;

public static class BoolExtensions
{
    public static string MapToTrueFalse(this bool? value)
    {
        return value switch
        {
            true => "True",
            false => "False",
            _ => string.Empty,
        };
    }

    public static string MapToTrueFalse(this bool value) => ((bool?)value).MapToTrueFalse();

    public static string MapToYesNo(this bool? value)
    {
        return value switch
        {
            true => "Yes",
            false => "No",
            _ => string.Empty,
        };
    }
}
