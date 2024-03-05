using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.Contract.Application.Helper;
using HE.Investments.Loans.Contract.ViewModels;

namespace HE.Investments.Loans.Contract.CompanyStructure;

public class CompanyStructureViewModel : ISectionViewModel
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

    public SectionStatus Status { get; set; }

    public bool StateChanged { get; set; }

    public IList<FileModel>? OrganisationMoreInformationFiles { get; set; }

    public string? AllowedExtensions { get; set; }

    public bool IsCompleted()
    {
        return Status == SectionStatus.Completed;
    }

    public bool IsProgress()
    {
        return Status == SectionStatus.InProgress;
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
