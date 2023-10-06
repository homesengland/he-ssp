using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.ViewModels;

namespace HE.InvestmentLoans.Contract.Security;

public class SecurityViewModel : ICompletedSectionViewModel
{
    public SecurityViewModel()
    {
        State = SectionStatus.NotStarted;
    }

    public Guid LoanApplicationId { get; set; }

    public ApplicationStatus LoanApplicationStatus { get; set; }

    public string? CheckAnswers { get; set; }

    public string? ChargesDebtCompany { get; set; }

    public string? ChargesDebtCompanyInfo { get; set; }

    public string? DirLoans { get; set; }

    public string? DirLoansSub { get; set; }

    public string? DirLoansSubMore { get; set; }

    public string? Name { get; set; }

    public SectionStatus State { get; set; }

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

    public bool IsCompleted()
    {
        return State == SectionStatus.Completed;
    }

    public bool IsInProgress()
    {
        return State == SectionStatus.InProgress;
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(LoanApplicationStatus);
    }

    public bool IsEditable() => IsReadOnly() is false;
}
