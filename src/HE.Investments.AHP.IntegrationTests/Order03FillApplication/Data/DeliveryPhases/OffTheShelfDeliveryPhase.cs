using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;

public sealed class OffTheShelfDeliveryPhase : DeliveryPhaseDataBase<OffTheShelfDeliveryPhase>
{
    public override TypeOfHomes TypeOfHomes => TypeOfHomes.NewBuild;

    public override BuildActivityType BuildActivityType => BuildActivityType.OffTheShelf;

    protected override OffTheShelfDeliveryPhase DeliveryPhase => this;
}
