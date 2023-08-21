namespace HE.InvestmentLoans.Contract.CompanyStructure;

public class CompanyStructureViewModel
{
    public CompanyStructureViewModel()
    {
        StateChanged = false;
    }

    public Guid LoanApplicationId { get; set; }

    public string? Purpose { get; set; }

    public string? ExistingCompany { get; set; }

    public string? ExistingCompanyInfo { get; set; }

    public string? HomesBuilt { get; set; }

    public string? CheckAnswers { get; set; }

    public byte[]? CompanyInfoFile { get; set; }

    public string? CompanyInfoFileName { get; set; }

    public CompanyStructureState State { get; set; }

    public bool StateChanged { get; set; }

    public bool IsFlowCompleted { get; set; }

    public void SetFlowCompletion(bool value)
    {
        IsFlowCompleted = value;
    }
}
