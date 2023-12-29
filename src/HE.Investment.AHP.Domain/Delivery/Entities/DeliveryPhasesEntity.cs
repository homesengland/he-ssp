using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhasesEntity
{
    private readonly IList<DeliveryPhaseEntity> _deliveryPhases;

    private readonly ApplicationBasicInfo _application;

    public DeliveryPhasesEntity(ApplicationBasicInfo application, IEnumerable<DeliveryPhaseEntity> deliveryPhases, SectionStatus status)
    {
        _application = application;
        _deliveryPhases = deliveryPhases.ToList();
        Status = status;
    }

    public ApplicationId ApplicationId => _application.Id;

    public ApplicationName ApplicationName => _application.Name;

    public IEnumerable<IDeliveryPhaseEntity> DeliveryPhases => _deliveryPhases;

    public SectionStatus Status { get; private set; }
}
