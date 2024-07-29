namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.DeliveryPhases;

public class DeliveryPhasesData
{
    public const decimal RehabHomesPercentage = 0.6m;

    public const decimal OffTheShelfHomesPercentage = 0.4m;

    public DeliveryPhasesData()
    {
        RehabDeliveryPhase = new RehabDeliveryPhase();
        OffTheShelfDeliveryPhase = new OffTheShelfDeliveryPhase();
    }

    public RehabDeliveryPhase RehabDeliveryPhase { get; }

    public OffTheShelfDeliveryPhase OffTheShelfDeliveryPhase { get; }

    public void GenerateHomes(IList<HomeTypeDetails> deliveryPhaseHomes)
    {
        var rehabHomes = deliveryPhaseHomes.Select(x => x with { NumberOfHomes = (int)(x.NumberOfHomes * RehabHomesPercentage) }).ToList();
        var offTheShelfHomes = deliveryPhaseHomes
            .Select(x => x with { NumberOfHomes = x.NumberOfHomes - rehabHomes.Single(y => y.Id == x.Id).NumberOfHomes })
            .ToList();

        RehabDeliveryPhase.DeliveryPhaseHomes = rehabHomes;
        OffTheShelfDeliveryPhase.DeliveryPhaseHomes = offTheShelfHomes;
    }
}
