using HE.Investments.Loans.Contract.Application.Enums;

namespace HE.Investments.Loans.Contract.ViewModels;

public interface ICompletedSectionViewModel : IEditableViewModel
{
    public string? CheckAnswers { get; set; }

    public ApplicationStatus GetApplicationStatus();
}
