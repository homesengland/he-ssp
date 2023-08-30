using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.Contract.CompanyStructure;

public class CompanyStructureViewModel
{
    public CompanyStructureViewModel()
    {
        StateChanged = false;
    }

    public Guid LoanApplicationId { get; set; }

    public string? Purpose { get; set; }

    public string? OrganisationMoreInformation { get; set; }

    public string? HomesBuilt { get; set; }

    public string? CheckAnswers { get; set; }

    public byte[]? OrganisationMoreInformationFile { get; set; }

    public string? CompanyInfoFileName { get; set; }

    public SectionStatus State { get; set; }

    public bool StateChanged { get; set; }

    public bool IsFlowCompleted { get; set; }

    public bool IsCompleted()
    {
        return State == SectionStatus.Completed;
    }

    public bool IsProgress()
    {
        return State == SectionStatus.InProgress;
    }
}
