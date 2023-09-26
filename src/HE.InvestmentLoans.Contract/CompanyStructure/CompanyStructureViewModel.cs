using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.ViewModels;

namespace HE.InvestmentLoans.Contract.CompanyStructure;

public class CompanyStructureViewModel : ICompletedSectionViewModel
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

    public bool IsCompleted()
    {
        return State == SectionStatus.Completed;
    }

    public bool IsProgress()
    {
        return State == SectionStatus.InProgress;
    }

    public bool IsEditable()
    {
        return true;
    }

    public bool IsReadOnly() => IsEditable() is false;
}
