using HE.Investment.AHP.Contract.Application;
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
        IReadOnlyCollection<AhpApplicationOperation> allowedOperations)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsCompleted = isCompleted;
        Section = section;
        AllowedOperations = allowedOperations;
    }

    public string ApplicationId { get; }

    public string ApplicationName { get; }

    public IsSectionCompleted? IsCompleted { get; }

    public SectionSummaryViewModel Section { get; }

    public IReadOnlyCollection<AhpApplicationOperation> AllowedOperations { get; }

    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
