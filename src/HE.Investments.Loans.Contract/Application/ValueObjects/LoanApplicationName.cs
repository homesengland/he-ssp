using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;

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
