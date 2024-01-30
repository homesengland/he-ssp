using System.Globalization;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class RehabDeliveryPhase : INestedItemData
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

    public RehabDeliveryPhase GenerateDeliveryPhase()
    {
        Name = new DeliveryPhaseName($"IT-Rehab-WorksOnly-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
        return this;
    }

    public RehabDeliveryPhase GenerateDetails()
    {
        TypeOfHomes = TypeOfHomes.Rehab;
        return this;
    }

    public RehabDeliveryPhase GenerateBuildActivityType()
    {
        BuildActivityType = BuildActivityType.WorksOnlyRehab;
        return this;
    }

    public RehabDeliveryPhase GenerateReconfiguringExisting()
    {
        ReconfiguringExisting = true;
        return this;
    }

    public RehabDeliveryPhase GenerateHomes(IDictionary<string, int> deliveryPhaseHomes)
    {
        DeliveryPhaseHomes = deliveryPhaseHomes;
        return this;
    }

    public RehabDeliveryPhase GenerateAcquisitionMilestone()
    {
        AcquisitionMilestone = new AcquisitionMilestoneDetails(
            new AcquisitionDate("12", "12", "2021"),
            new MilestonePaymentDate("13", "7", "2022"));
        return this;
    }

    public RehabDeliveryPhase GenerateStartOnSiteMilestone()
    {
        StartOnSiteMilestone = new StartOnSiteMilestoneDetails(
            new StartOnSiteDate("7", "7", "2022"),
            new MilestonePaymentDate("8", "8", "2022"));
        return this;
    }

    public RehabDeliveryPhase GenerateCompletionMilestone()
    {
        CompletionMilestone = new CompletionMilestoneDetails(
            new CompletionDate("9", "9", "2025"),
            new MilestonePaymentDate("10", "10", "2025"));
        return this;
    }
}
