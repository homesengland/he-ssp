using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.Contract.ViewModels;

public interface ICompletedSectionViewModel : IEditableViewModel
{
    public string? CheckAnswers { get; set; }

    public ApplicationStatus GetApplicationStatus();
}
