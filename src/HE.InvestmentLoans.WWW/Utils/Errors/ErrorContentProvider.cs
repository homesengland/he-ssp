using System.Globalization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Infrastructure.ErrorHandling;
using HE.InvestmentLoans.Contract;

namespace HE.InvestmentLoans.WWW.Utils.Errors;

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

        var ukDate = utcDate.ConvertUtcToUkLocalTime();

        return ("This application has already been submitted", $"Application submitted at {ukDate.ToString("hh:mm tt", CultureInfo.InvariantCulture)} on {ukDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
    }
}
