using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.WWW.Models;

public class WithdrawModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public string WithdrawReason { get; set; }

    public string ApplicationStatus { get; set; }
}
