using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhaseEntity : IDeliveryPhaseEntity
{
    public DeliveryPhaseEntity(
        ApplicationBasicInfo application,
        string? name,
        SectionStatus status,
        DeliveryPhaseId? id = null,
        DateTime? createdOn = null)
    {
        Application = application;
        Name = new DeliveryPhaseName(name);
        Status = status;
        Id = id ?? DeliveryPhaseId.New();
        CreatedOn = createdOn;
    }

    public ApplicationBasicInfo Application { get; }

    public DeliveryPhaseId Id { get; set; }

    public DeliveryPhaseName Name { get; private set; }

    public DateTime? CreatedOn { get; }

    public SectionStatus Status { get; private set; }
}
