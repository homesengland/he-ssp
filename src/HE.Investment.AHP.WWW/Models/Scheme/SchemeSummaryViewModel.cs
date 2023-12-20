using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public class SchemeSummaryViewModel : IEditableViewModel
{
    public SchemeSummaryViewModel(string applicationId, string applicationName, bool? isCompleted, SectionSummaryViewModel section, bool isEditable)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsCompleted = isCompleted;
        Section = section;
        IsEditable = isEditable;
    }

    public string ApplicationId { get; }

    public string ApplicationName { get; }

    public bool? IsCompleted { get; }

    public SectionSummaryViewModel Section { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
