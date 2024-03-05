using HE.Investments.Common.Contract;

namespace HE.Investments.Loans.Contract.ViewModels;

public interface ISectionViewModel : ICompletedSectionViewModel
{
    public SectionStatus Status { get; set; }
}
