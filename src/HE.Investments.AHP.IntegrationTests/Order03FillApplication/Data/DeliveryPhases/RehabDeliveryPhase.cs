using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;

public sealed class RehabDeliveryPhase : DeliveryPhaseDataBase<RehabDeliveryPhase>
{
    public override TypeOfHomes TypeOfHomes => TypeOfHomes.Rehab;

    public override BuildActivityType BuildActivityType => BuildActivityType.WorksOnlyRehab;

    public DateDetails AcquisitionMilestoneDate { get; private set; }

    public DateDetails AcquisitionMilestonePaymentDate { get; private set; }

    public DateDetails StartOnSiteMilestoneDate { get; private set; }

    public DateDetails StartOnSiteMilestonePaymentDate { get; private set; }

    public DateDetails InvalidCompletionMilestonePaymentDate { get; private set; }

    public bool ReconfiguringExisting { get; private set; }

    protected override RehabDeliveryPhase DeliveryPhase => this;

    public RehabDeliveryPhase GenerateReconfiguringExisting()
    {
        ReconfiguringExisting = true;
        return this;
    }

    public RehabDeliveryPhase GenerateAcquisitionMilestone()
    {
        AcquisitionMilestoneDate = new DateDetails("29", "11", "2025");
        AcquisitionMilestonePaymentDate = new DateDetails("01", "01", "2026");
        return this;
    }

    public RehabDeliveryPhase GenerateStartOnSiteMilestone()
    {
        StartOnSiteMilestoneDate = new DateDetails("30", "11", "2025");
        StartOnSiteMilestonePaymentDate = new DateDetails("01", "02", "2026");
        return this;
    }

    public override RehabDeliveryPhase GenerateCompletionMilestone()
    {
        InvalidCompletionMilestonePaymentDate = new DateDetails("01", "01", "2030");
        return base.GenerateCompletionMilestone();
    }
}
