using HE.Investments.Common.Contract;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Application.Helper;
using HE.Investments.Loans.Contract.ViewModels;

namespace HE.Investments.Loans.Contract.Security;

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

    public bool IsEditable() => !IsReadOnly();

    public ApplicationStatus GetApplicationStatus()
    {
        return LoanApplicationStatus;
    }
}
