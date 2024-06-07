using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.Site;

public class SiteSummaryViewModel : ISummaryViewModel
{
    public SiteSummaryViewModel(string siteId, SiteStatus siteStatus, IsSectionCompleted? isSectionCompleted, IList<SectionSummaryViewModel> sections, bool isEditable)
    {
        SiteId = siteId;
        SiteStatus = siteStatus;
        IsSectionCompleted = isSectionCompleted;
        Sections = sections;
        IsEditable = isEditable;
    }

    public string SiteId { get; }

    public SiteStatus SiteStatus { get; }

    public IsSectionCompleted? IsSectionCompleted { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
