using HE.Investments.Common.Contract;

namespace HE.Investments.Loans.Contract.ViewModels;

public interface ICompletedSectionViewModel : IEditableViewModel
{
    public string? CheckAnswers { get; set; }

    public ApplicationStatus GetApplicationStatus();
}
