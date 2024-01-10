using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public interface IDeliveryPhaseEntity
{
    ApplicationBasicInfo Application { get; }

    public OrganisationBasicInfo Organisation { get; }

    DeliveryPhaseId Id { get; }

    DeliveryPhaseName? Name { get; }

    TypeOfHomes? TypeOfHomes { get; }

    BuildActivityTypeForNewBuild? BuildActivityTypeForNewBuild { get; }

    BuildActivityTypeForRehab? BuildActivityTypeForRehab { get; }

    DateTime? CreatedOn { get; }

    SectionStatus Status { get; }

    int TotalHomesToBeDeliveredInThisPhase { get; }

    AcquisitionMilestoneDetails? AcquisitionMilestone { get; }

    StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; }

    CompletionMilestoneDetails? CompletionMilestone { get; }

    public IsAdditionalPaymentRequested? IsAdditionalPaymentRequested { get; }

    void ProvideName(DeliveryPhaseName deliveryPhaseName);

    void ProvideAcquisitionMilestoneDetails(AcquisitionMilestoneDetails? details);

    void ProvideStartOnSiteMilestoneDetails(StartOnSiteMilestoneDetails? details);

    void ProvideCompletionMilestoneDetails(CompletionMilestoneDetails? details);

    void ProvideAdditionalPaymentRequest(IsAdditionalPaymentRequested? isAdditionalPaymentRequested);

    void ProvideTypeOfHomes(TypeOfHomes typeOfHomes);

    void ProvideBuildActivityType(BuildActivityTypeForNewBuild requestBuildActivityType);
}
