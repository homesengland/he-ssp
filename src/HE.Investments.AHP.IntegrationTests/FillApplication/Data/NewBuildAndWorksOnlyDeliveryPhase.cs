using System.Globalization;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class NewBuildAndWorksOnlyDeliveryPhase : INestedItemData
{
    public string Id { get; private set; }

    public DeliveryPhaseName Name { get; private set; }

    public TypeOfHomes TypeOfHomes { get; private set; }

    public BuildActivityType BuildActivityType { get; private set; }

    public AcquisitionMilestoneDetails AcquisitionMilestone { get; private set; }

    public StartOnSiteMilestoneDetails StartOnSiteMilestone { get; private set; }

    public CompletionMilestoneDetails CompletionMilestone { get; private set; }

    public void SetDeliveryPhaseId(string deliveryPhaseId)
    {
        Id = deliveryPhaseId;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateDeliveryPhase()
    {
        Name = new DeliveryPhaseName($"IT-General-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateDetails()
    {
        TypeOfHomes = TypeOfHomes.NewBuild;
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateBuildActivityType()
    {
        BuildActivityType = new BuildActivityType(TypeOfHomes, BuildActivityTypeForNewBuild.WorksOnly);
        return this;
    }
}
