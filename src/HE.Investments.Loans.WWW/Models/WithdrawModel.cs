using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.WWW.Models;

public class WithdrawModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public string WithdrawReason { get; set; }
}
