using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public interface IDeliveryPhaseEntity
{
    ApplicationBasicInfo Application { get; }

    DeliveryPhaseId Id { get; }

    DeliveryPhaseName Name { get; }

    DateTime? CreatedOn { get; }

    SectionStatus Status { get; }
}
