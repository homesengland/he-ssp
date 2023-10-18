using HE.InvestmentLoans.Contract.Common;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class LoanApplicationName : ShortText
{
    public LoanApplicationName(string? value)
        : base(value, nameof(LoanApplicationName))
    {
    }
}
