using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW.Models.Application;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public class DeliveryPhaseSummaryViewModel
{
    public DeliveryPhaseSummaryViewModel(string applicationId, string deliveryPhaseId, string applicationName, string deliveryPhaseName, IsSectionCompleted? isCompleted, IList<SectionSummaryViewModel> sections, bool isEditable)
    {
        ApplicationId = applicationId;
        DeliveryPhaseId = deliveryPhaseId;
        ApplicationName = applicationName;
        DeliveryPhaseName = deliveryPhaseName;
        IsCompleted = isCompleted;
        Sections = sections;
        IsEditable = isEditable;
    }

    public string ApplicationId { get; }

    public string DeliveryPhaseId { get; }

    public string ApplicationName { get; }

    public string DeliveryPhaseName { get; }

    public IsSectionCompleted? IsCompleted { get; }

    public IList<SectionSummaryViewModel> Sections { get; }

    public bool IsEditable { get; }

    public bool IsReadOnly => !IsEditable;
}
