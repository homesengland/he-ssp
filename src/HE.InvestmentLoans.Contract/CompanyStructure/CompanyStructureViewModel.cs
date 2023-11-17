using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.ViewModels;
using HE.Investments.Common.Domain;
using HE.Investments.DocumentService.Models.File;
using ApplicationStatus = HE.InvestmentLoans.Contract.Application.Enums.ApplicationStatus;

namespace HE.InvestmentLoans.Contract.CompanyStructure;

public class CompanyStructureViewModel : ICompletedSectionViewModel
{
    public CompanyStructureViewModel()
    {
        StateChanged = false;
    }

    public Guid LoanApplicationId { get; set; }

    public ApplicationStatus LoanApplicationStatus { get; set; }

    public string? Purpose { get; set; }

    public string? OrganisationMoreInformation { get; set; }

    public string? HomesBuilt { get; set; }

    public string? CheckAnswers { get; set; }

    public byte[]? OrganisationMoreInformationFile { get; set; }

    public string? CompanyInfoFileName { get; set; }

    public SectionStatus State { get; set; }

    public bool StateChanged { get; set; }

    public IList<FileTableRow>? OrganisationMoreInformationFiles { get; set; }

    public IList<string> AllowedFileExtensions { get; set; }

    public bool IsCompleted()
    {
        return State == SectionStatus.Completed;
    }

    public bool IsProgress()
    {
        return State == SectionStatus.InProgress;
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(LoanApplicationStatus);
    }

    public bool IsEditable() => IsReadOnly() is false;

    public ApplicationStatus GetApplicationStatus()
    {
        return LoanApplicationStatus;
    }
}
