using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.DeliveryPhases;

public sealed class OffTheShelfDeliveryPhase : DeliveryPhaseDataBase<OffTheShelfDeliveryPhase>
{
    public override TypeOfHomes TypeOfHomes => TypeOfHomes.NewBuild;

    public override BuildActivityType BuildActivityType => BuildActivityType.OffTheShelf;

    protected override OffTheShelfDeliveryPhase DeliveryPhase => this;

    public override void PopulateAllData()
    {
        GenerateDeliveryPhase();
        GenerateCompletionMilestone();
    }
}
