using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
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
        return new DeliveryPhasesEntity(
            new ApplicationBasicInfo(
                new AhpApplicationId("test-app-42123"),
                new ApplicationName("Test Application"),
                Tenure.AffordableRent,
                ApplicationStatus.Draft),
            _deliveryPhases,
            _homesToDelivers,
            _status);
    }
}
