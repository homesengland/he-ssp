using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using Microsoft.AspNetCore.Server.IIS.Core;

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

    public static bool MapToNonNullableBool(this string value)
    {
        if (value == CommonResponse.Yes)
        {
            return true;
        }

        if (value == CommonResponse.No)
        {
            return false;
        }

        throw new ArgumentException($"\"{value}\" cannot be converted to bool. Only \"Yes\" or \"No\" can be converted using this method.");
    }

    public static string MapToCommonResponse(this bool? value)
    {
        if (value is null)
        {
            return null!;
        }

        return MapToCommonResponse(value.Value);
    }

    public static string MapToCommonResponse(this bool value)
    {
        return value ? CommonResponse.Yes : CommonResponse.No;
    }
}
