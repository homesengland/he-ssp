using HE.Investments.Common.Contract.Constants;
using HE.Investments.Loans.Contract.Application.Enums;

namespace HE.Investments.Loans.Contract.Application.Extensions;
public static class YesNoAnswersExtensions
{
    public static YesNoAnswers ToYesNoAnswer(this string value) => value switch
    {
        CommonResponse.Yes => YesNoAnswers.Yes,
        CommonResponse.No => YesNoAnswers.No,
        _ => YesNoAnswers.Undefined,
    };
}
