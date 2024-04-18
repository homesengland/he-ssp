using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class DeliveryPhasesEntityBuilder
{
    private readonly IList<HomesToDeliver> _homesToDelivers = new List<HomesToDeliver>();

    private readonly IList<DeliveryPhaseEntity> _deliveryPhases = new List<DeliveryPhaseEntity>();

    private SectionStatus _status = SectionStatus.NotStarted;

    public DeliveryPhasesEntityBuilder WithHomesToDeliver(HomesToDeliver homesToDeliver)
    {
        _homesToDelivers.Add(homesToDeliver);
        return this;
    }

    public DeliveryPhasesEntityBuilder WithDeliveryPhase(DeliveryPhaseEntity deliveryPhase)
    {
        _deliveryPhases.Add(deliveryPhase);
        return this;
    }

    public DeliveryPhasesEntityBuilder WithStatus(SectionStatus status)
    {
        _status = status;
        return this;
    }

    public DeliveryPhasesEntity Build()
    {
        return new DeliveryPhasesEntity(ApplicationBasicInfoTestData.AffordableRentInDraftState, _deliveryPhases, _homesToDelivers, _status);
    }
}
