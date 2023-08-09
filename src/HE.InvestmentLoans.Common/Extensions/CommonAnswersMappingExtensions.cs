using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.Common.Extensions;
public static class CommonAnswersMappingExtensions
{
    public static bool? MapToBool(this string value)
    {
        if (value == CommonResponse.Yes)
        {
            return true;
        }

        if (value == CommonResponse.No)
        {
            return false;
        }

        return null;
    }

    public static string MapToCommonResponse(this bool? value)
    {
        if (value is null)
        {
            return null!;
        }

        return value.Value ? CommonResponse.Yes : CommonResponse.No;
    }
}
