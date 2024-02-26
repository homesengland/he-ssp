using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Application;

public interface ISummaryViewModel : IEditableViewModel
{
    public IList<SectionSummaryViewModel> Sections { get; }
}
