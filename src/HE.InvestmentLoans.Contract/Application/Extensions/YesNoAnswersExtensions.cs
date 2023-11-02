using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.Contract.Application.Extensions;
public static class YesNoAnswersExtensions
{
    public static YesNoAnswers ToYesNoAnswer(this string value) => value switch
    {
        CommonResponse.Yes => YesNoAnswers.Yes,
        CommonResponse.No => YesNoAnswers.No,
        _ => YesNoAnswers.Undefined,
    };
}
