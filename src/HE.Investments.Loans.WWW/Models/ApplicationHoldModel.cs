using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.WWW.Models;

public class ApplicationHoldModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public string HoldReason { get; set; }
}
