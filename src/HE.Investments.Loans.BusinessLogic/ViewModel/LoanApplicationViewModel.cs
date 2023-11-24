using HE.Investments.Loans.BusinessLogic.LoanApplication;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.CompanyStructure;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.Contract.Security;

namespace HE.Investments.Loans.BusinessLogic.ViewModel;

public class LoanApplicationViewModel
{
    public LoanApplicationViewModel()
    {
        ID = Guid.NewGuid();
        State = LoanApplicationWorkflow.State.AboutLoan;
    }

    public Guid ID { get; set; }

    public ApplicationStatus Status { get; set; }

    public LoanApplicationWorkflow.State State
    {
        get;
        set;
    }

    public DateTime? Timestamp { get; set; }

    public CompanyStructureViewModel Company { get; set; }

    public FundingViewModel Funding { get; set; }

    public SecurityViewModel Security { get; set; }

    public IEnumerable<ProjectViewModel> Projects { get; set; }

    public FundingPurpose? Purpose { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? WithdrawReason { get; set; }

    public bool WasSubmittedPreviously { get; set; }

    public void SetTimestamp(DateTime? timestamp)
    {
        Timestamp = timestamp;
    }
}
