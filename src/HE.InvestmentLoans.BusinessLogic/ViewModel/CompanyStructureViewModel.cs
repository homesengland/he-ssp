using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class CompanyStructureViewModel
{
    public CompanyStructureViewModel()
    {
        State = CompanyStructureWorkflow.State.Index;
        StateChanged = false;
    }

    public string? Purpose { get; set; }

    public string? ExistingCompany { get; set; }

    public string? ExistingCompanyInfo { get; set; }

    public string? HomesBuilt { get; set; }

    public string? CheckAnswers { get; set; }

    public byte[]? CompanyInfoFile { get; set; }

    public string? CompanyInfoFileName { get; set; }

    public CompanyStructureWorkflow.State State { get; set; }

    public bool StateChanged { get; set; }
}
