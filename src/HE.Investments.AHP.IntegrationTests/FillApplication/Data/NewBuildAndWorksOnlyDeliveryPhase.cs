using System.Globalization;
using HE.Investment.AHP.Contract.Delivery.Enums;
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

    public bool ReconfiguringExisting { get; private set; }

    public IDictionary<string, int> DeliveryPhaseHomes { get; private set; }

    public void SetDeliveryPhaseId(string deliveryPhaseId)
    {
        Id = deliveryPhaseId;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateDeliveryPhase()
    {
        Name = new DeliveryPhaseName($"IT-NewBuild-WorksOnly-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateDetails()
    {
        TypeOfHomes = TypeOfHomes.Rehab;
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateBuildActivityType()
    {
        BuildActivityType = BuildActivityType.WorksOnlyRehab;
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateReconfiguringExisting()
    {
        ReconfiguringExisting = true;
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateHomes(IDictionary<string, int> deliveryPhaseHomes)
    {
        DeliveryPhaseHomes = deliveryPhaseHomes;
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateAcquisitionMilestone()
    {
        AcquisitionMilestone = new AcquisitionMilestoneDetails(
            new AcquisitionDate("12", "12", "2021"),
            new MilestonePaymentDate("13", "7", "2022"));
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateStartOnSiteMilestone()
    {
        StartOnSiteMilestone = new StartOnSiteMilestoneDetails(
            new StartOnSiteDate("7", "7", "2022"),
            new MilestonePaymentDate("8", "8", "2022"));
        return this;
    }

    public NewBuildAndWorksOnlyDeliveryPhase GenerateCompletionMilestone()
    {
        CompletionMilestone = new CompletionMilestoneDetails(
            new CompletionDate("9", "12", "2026"),
            new MilestonePaymentDate("20", "1", "2026"));
        return this;
    }
}
