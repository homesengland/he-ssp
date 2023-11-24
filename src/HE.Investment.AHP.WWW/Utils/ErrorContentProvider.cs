using HE.Investments.Loans.Common.Infrastructure.ErrorHandling;

namespace HE.Investment.AHP.WWW.Utils;

public static class ErrorContentProvider
{
    public static (string? Header, string? Body) GetContent(ErrorModel errorModel)
    {
        return errorModel.ErrorCode switch
        {
            _ => (null, null),
        };
    }
}
