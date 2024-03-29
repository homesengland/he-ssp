using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.Site;

public class SiteSummaryViewModel : ISummaryViewModel
{
    public SiteSummaryViewModel(string siteId, IsSectionCompleted? isSectionCompleted, IList<SectionSummaryViewModel> sections, bool isEditable)
    {
        SiteId = siteId;
        IsSectionCompleted = isSectionCompleted;
        Sections = sections;
        IsEditable = isEditable;
    }

    public string SiteId { get; }

    public IsSectionCompleted? IsSectionCompleted { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
