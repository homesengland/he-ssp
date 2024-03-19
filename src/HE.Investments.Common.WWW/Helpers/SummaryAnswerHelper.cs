using System.Globalization;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.WWW.Helpers;

public static class SummaryAnswerHelper
{
    public static IList<string>? ToYesNo(bool? answer, string? additionalText = null)
    {
        if (answer.IsNotProvided())
        {
            return null;
        }

        var yesNoAnswer = (answer!.Value ? YesNoType.Yes : YesNoType.No).GetDescription();
        return additionalText.IsProvided() ? $"{yesNoAnswer}, {additionalText}".ToOneElementList() : yesNoAnswer.ToOneElementList();
    }

    public static IList<string>? ToEnum<TEnum>(TEnum? enumValue)
        where TEnum : struct, Enum
    {
        return enumValue == null || Convert.ToInt32(enumValue, CultureInfo.InvariantCulture) == 0 ? null : enumValue.Value.GetDescription().ToOneElementList();
    }

    public static IList<string>? ToEnum<TEnum>(TEnum enumValue)
        where TEnum : struct, Enum
    {
        return ToEnum((TEnum?)enumValue);
    }

    public static IList<string>? ToDate(DateDetails? date) => DateHelper.DisplayAsUkFormatDate(date)?.ToOneElementList();
}
