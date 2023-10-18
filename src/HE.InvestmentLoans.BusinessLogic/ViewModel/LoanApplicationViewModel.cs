using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.Funding;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.Contract.Security;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

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

    public AccountDetailsViewModel Account { get; set; }

    public FundingPurpose? Purpose { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? WithdrawReason { get; set; }

    public void SetTimestamp(DateTime? timestamp)
    {
        Timestamp = timestamp;
    }
}
