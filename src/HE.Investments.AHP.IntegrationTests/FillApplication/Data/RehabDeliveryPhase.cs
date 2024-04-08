using System.Globalization;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class RehabDeliveryPhase : INestedItemData
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    public TypeOfHomes TypeOfHomes { get; private set; }

    public BuildActivityType BuildActivityType { get; private set; }

    public DateDetails AcquisitionMilestoneDate { get; private set; }

    public DateDetails AcquisitionMilestonePaymentDate { get; private set; }

    public DateDetails StartOnSiteMilestoneDate { get; private set; }

    public DateDetails StartOnSiteMilestonePaymentDate { get; private set; }

    public DateDetails CompletionMilestoneDate { get; private set; }

    public DateDetails CompletionMilestonePaymentDate { get; private set; }

    public bool ReconfiguringExisting { get; private set; }

    public IList<HomeTypeDetails> DeliveryPhaseHomes { get; private set; }

    public void SetDeliveryPhaseId(string deliveryPhaseId)
    {
        Id = deliveryPhaseId;
    }

    public RehabDeliveryPhase GenerateDeliveryPhase()
    {
        Name = $"IT-Rehab-WorksOnly-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
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

    public RehabDeliveryPhase GenerateHomes(IList<HomeTypeDetails> deliveryPhaseHomes)
    {
        DeliveryPhaseHomes = deliveryPhaseHomes;
        return this;
    }

    public RehabDeliveryPhase GenerateAcquisitionMilestone()
    {
        AcquisitionMilestoneDate = new DateDetails("12", "12", "2021");
        AcquisitionMilestonePaymentDate = new DateDetails("13", "7", "2022");
        return this;
    }

    public RehabDeliveryPhase GenerateStartOnSiteMilestone()
    {
        StartOnSiteMilestoneDate = new DateDetails("7", "7", "2022");
        StartOnSiteMilestonePaymentDate = new DateDetails("8", "8", "2022");
        return this;
    }

    public RehabDeliveryPhase GenerateCompletionMilestone()
    {
        CompletionMilestoneDate = new DateDetails("9", "9", "2023");
        CompletionMilestonePaymentDate = new DateDetails("10", "10", "2023");
        return this;
    }
}
