using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public interface IDeliveryPhaseEntity
{
    ApplicationBasicInfo Application { get; }

    DeliveryPhaseId Id { get; }

    DeliveryPhaseName? Name { get; }

    DateTime? CreatedOn { get; }

    SectionStatus Status { get; }

    int TotalHomesToBeDeliveredInThisPhase { get; }

    AcquisitionMilestoneDetails? AcquisitionMilestone { get; }

    StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; }

    CompletionMilestoneDetails? CompletionMilestone { get; }

    void ProvideName(DeliveryPhaseName deliveryPhaseName);

    void ProvideAcquisitionMilestoneDetails(AcquisitionMilestoneDetails? details);

    void ProvideStartOnSiteMilestoneDetails(StartOnSiteMilestoneDetails? details);

    void ProvideCompletionMilestoneDetails(CompletionMilestoneDetails? details);
}
