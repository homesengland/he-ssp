namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public sealed class StartOnSiteMilestoneDetails : MilestoneDetails<StartOnSiteDate>
{
    private StartOnSiteMilestoneDetails(StartOnSiteDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static StartOnSiteMilestoneDetails? Create(StartOnSiteDate? milestoneDate, MilestonePaymentDate? paymentDate)
    {
        return milestoneDate?.Value != null || paymentDate?.Value != null ? new StartOnSiteMilestoneDetails(milestoneDate, paymentDate) : null;
    }
}
