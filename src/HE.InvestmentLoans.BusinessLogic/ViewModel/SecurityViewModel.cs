using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class SecurityViewModel
{
    public SecurityViewModel()
    {
        State = SecurityWorkflow.State.Index;
    }

    public string? CheckAnswers { get; set; }

    public string? ChargesDebtCompany { get; set; }

    public string? ChargesDebtCompanyInfo { get; set; }

    public string? DirLoans { get; set; }

    public string? DirLoansSub { get; set; }

    public string? DirLoansSubMore { get; set; }

    public string? Name { get; set; }

    public SecurityWorkflow.State State { get; set; }

    public bool StateChanged { get; set; }

    public bool IsFlowCompleted { get; set; }

    public void RemoveAlternativeRoutesData()
    {
        if (ChargesDebtCompany == CommonResponse.No)
        {
            ChargesDebtCompanyInfo = null;
        }

        if (DirLoans == CommonResponse.No)
        {
            DirLoansSub = null;
            DirLoansSubMore = null;
        }
        else if (DirLoansSub == CommonResponse.Yes)
        {
            DirLoansSubMore = null;
        }
    }

    public void SetFlowCompletion(bool value)
    {
        IsFlowCompleted = value;
    }
}
