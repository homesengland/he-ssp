using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class FormAnswerExtensions
{
    public static string ToBoolAnswer(this YesNoType yesNoType)
    {
        return yesNoType switch
        {
            YesNoType.Yes => "True",
            YesNoType.No => "False",
            _ => throw new ArgumentOutOfRangeException(nameof(yesNoType), yesNoType, null),
        };
    }

    public static (string InputName, string Value)[] ToFormInputs<TEnum>(this IEnumerable<TEnum> values, string fieldName)
        where TEnum : struct, Enum
    {
        return values.Select(x => (fieldName, x.ToString())).ToArray();
    }
}
