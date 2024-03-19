using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investments.FrontDoor.WWW.Models;

public class ProjectSummaryViewModel : ISummaryViewModel
{
    public ProjectSummaryViewModel(string projectId, IList<SectionSummaryViewModel> sections, bool? isSiteIdentified, bool isEditable)
    {
        ProjectId = projectId;
        Sections = sections;
        IsSiteIdentified = isSiteIdentified;
        IsEditable = isEditable;
    }

    public string ProjectId { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public bool? IsSiteIdentified { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
