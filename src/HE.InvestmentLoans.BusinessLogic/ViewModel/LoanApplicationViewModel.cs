using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class LoanApplicationViewModel
{
    public LoanApplicationViewModel()
    {
        Company = new CompanyStructureViewModel();
        Funding = new FundingViewModel();
        Security = new SecurityViewModel();
        Sites = new List<SiteViewModel>();
        Projects = new List<Project>();
        ID = Guid.NewGuid();
        State = LoanApplicationWorkflow.State.AboutLoan;
        Account = TemporaryAccount();
    }

    public Guid ID { get; set; }

    public LoanApplicationWorkflow.State State
    {
        get;
        set;
    }

    public DateTime Timestamp { get; set; }

    public CompanyStructureViewModel Company { get; set; }

    public FundingViewModel Funding { get; set; }

    public SecurityViewModel Security { get; set; }

    public List<SiteViewModel> Sites { get; set; }

    public List<Project> Projects { get; set; }

    public AccountDetailsViewModel Account { get; set; }

    public FundingPurpose? Purpose { get; set; }

    public SiteViewModel AddNewSite()
    {
        var site = new SiteViewModel
        {
            Id = Guid.NewGuid(),
            DefaultName = "New project",
        };

        Sites.Add(site);

        return site;
    }

    public Tuple<bool, string> ToggleDeleteProjectName(ICacheService cacheService, string passedDeleteProjectName = "")
    {
        var deleteProjectKey = "DeleteProject";
        var isDeletedProjectInCache = true;
        var deletedProjectFromCache = cacheService.GetValue<string>(deleteProjectKey) ?? string.Empty;

        if (string.IsNullOrEmpty(deletedProjectFromCache))
        {
            isDeletedProjectInCache = false;
        }

        cacheService.SetValue(deleteProjectKey, passedDeleteProjectName);

        return Tuple.Create(isDeletedProjectInCache, deletedProjectFromCache);
    }

    public void SetTimestamp(DateTime timestamp)
    {
        Timestamp = timestamp;
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
