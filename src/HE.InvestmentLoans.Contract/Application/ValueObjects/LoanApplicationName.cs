using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Common;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class LoanApplicationName : ShortText
{
    public LoanApplicationName(string? value)
        : base(value, nameof(LoanApplicationName), ValidationErrorMessage.EnterLoanApplicationName)
    {
    }

    public static LoanApplicationName CreateOrDefault(string? name)
    {
        return new LoanApplicationName(string.IsNullOrWhiteSpace(name) ? "Not provided" : name);
    }
}
