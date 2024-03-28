using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.Scheme;

public class SchemeSummaryViewModel : IEditableViewModel
{
    public SchemeSummaryViewModel(
        string applicationId,
        string applicationName,
        IsSectionCompleted? isCompleted,
        SectionSummaryViewModel section,
        bool isEditable,
        bool isApplicationLocked)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsCompleted = isCompleted;
        Section = section;
        IsEditable = isEditable;
        IsApplicationLocked = isApplicationLocked;
    }

    public string ApplicationId { get; }

    public string ApplicationName { get; }

    public IsSectionCompleted? IsCompleted { get; }

    public SectionSummaryViewModel Section { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;

    public bool IsApplicationLocked { get; }
}
