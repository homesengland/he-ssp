using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models.Summary;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public class DeliveryPhaseSummaryViewModel : ISummaryViewModel
{
    public DeliveryPhaseSummaryViewModel(
        string applicationId,
        string deliveryPhaseId,
        string applicationName,
        string deliveryPhaseName,
        IsSectionCompleted? isCompleted,
        IList<SectionSummaryViewModel> sections,
        IReadOnlyCollection<AhpApplicationOperation> allowedOperations)
    {
        ApplicationId = applicationId;
        DeliveryPhaseId = deliveryPhaseId;
        ApplicationName = applicationName;
        DeliveryPhaseName = deliveryPhaseName;
        IsCompleted = isCompleted;
        Sections = sections;
        AllowedOperations = allowedOperations;
    }

    public string ApplicationId { get; }

    public string DeliveryPhaseId { get; }

    public string ApplicationName { get; }

    public string DeliveryPhaseName { get; }

    public IsSectionCompleted? IsCompleted { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public IReadOnlyCollection<AhpApplicationOperation> AllowedOperations { get; }

    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;
}
