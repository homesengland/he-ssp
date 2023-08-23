using HE.InvestmentLoans.Common.Models.Others;
using HE.InvestmentLoans.Contract;

namespace HE.InvestmentLoans.WWW.Utils.Errors;

public static class ErrorContentProvider
{
    public static (string Header, string Body) GetContent(ErrorModel errorModel)
    {
        return errorModel.ErrorCode switch
        {
            CommonErrorCodes.ApplicationHasBeenSubmitted => ("This application has already been submitted", $"Application submitted at {errorModel.AdditionalData["Hour"]} on {errorModel.AdditionalData["Date"]}"),
            _ => (null, null),
        };
    }
}
