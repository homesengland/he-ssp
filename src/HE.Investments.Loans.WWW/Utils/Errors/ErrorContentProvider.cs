using HE.Investments.Common.Errors;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

namespace HE.Investments.Loans.WWW.Utils.Errors;

public static class ErrorContentProvider
{
    public static (string Header, string Body) GetContent(ErrorModel errorModel)
    {
        return errorModel.ErrorCode switch
        {
            CommonErrorCodes.ApplicationHasBeenSubmitted => ApplicationHasBeenSubmittedError(errorModel),
            _ => (null, null),
        };
    }

    private static (string Header, string Body) ApplicationHasBeenSubmittedError(ErrorModel errorModel)
    {
        var utcDate = (DateTime)errorModel.AdditionalData["Date"];

        return ("This application has already been submitted", $"Application submitted at {DateHelper.DisplayAsUkFormatDateTime(utcDate)}");
    }
}
