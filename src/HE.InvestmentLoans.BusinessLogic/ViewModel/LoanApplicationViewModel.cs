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
        Funding = new FundingViewModel();
        Projects = new List<ProjectViewModel>();
        ID = Guid.NewGuid();
        State = LoanApplicationWorkflow.State.AboutLoan;
        Account = TemporaryAccount();
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

    public bool GoodChangeMode { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? WithdrawReason { get; set; }

    public void SetTimestamp(DateTime? timestamp)
    {
        Timestamp = timestamp;
    }

    public void UseSectionsFrom(LoanApplicationViewModel model)
    {
        Projects = model.Projects;
        Security = model.Security;
        Funding = model.Funding;
        Timestamp = model.Timestamp;
    }

    public bool IsReadyToSubmit()
    {
        return (Funding.IsCompleted() || Funding.IsFlowCompleted)
            && Projects.All(x => x.Status == SectionStatus.Completed)
            && Projects.Any();
    }

    private AccountDetailsViewModel TemporaryAccount() => new()
    {
        RegisteredName = "ABC Developments",
        RegistrationNumber = "AC012345",
        Address = new AddressViewModel
        {
            City = "Leeds",
            Country = "United Kingdom",
            Postcode = "L21 37W",
            Street = "12 Wharf Street",
        },
        ContactName = "Johan Aswani",
        EmailAddress = "example@mail.com",
        TelephoneNumber = "01238 956738",
    };
}
