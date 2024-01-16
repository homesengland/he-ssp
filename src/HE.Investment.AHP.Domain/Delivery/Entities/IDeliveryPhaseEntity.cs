using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public interface IDeliveryPhaseEntity
{
    ApplicationBasicInfo Application { get; }

    Tenure Tenure => Application.Tenure;

    OrganisationBasicInfo Organisation { get; }

    DeliveryPhaseId Id { get; }

    DeliveryPhaseName? Name { get; }

    TypeOfHomes? TypeOfHomes { get; }

    BuildActivity BuildActivity { get; }

    bool? ReconfiguringExisting { get; }

    DateTime? CreatedOn { get; }

    SectionStatus Status { get; }

    int TotalHomesToBeDeliveredInThisPhase { get; }

    DeliveryPhaseMilestones DeliveryPhaseMilestones { get; }

    public IsAdditionalPaymentRequested? IsAdditionalPaymentRequested { get; }

    void ProvideName(DeliveryPhaseName deliveryPhaseName);

    Task ProvideDeliveryPhaseMilestones(DeliveryPhaseMilestones milestones, IMilestoneDatesInProgrammeDateRangePolicy policy);

    void ProvideAdditionalPaymentRequest(IsAdditionalPaymentRequested? isAdditionalPaymentRequested);

    void ProvideTypeOfHomes(TypeOfHomes typeOfHomes);

    void ProvideBuildActivity(BuildActivity buildActivity);

    void ProvideReconfiguringExisting(bool? reconfiguringExisting);

    bool IsReconfiguringExistingNeeded();
}
