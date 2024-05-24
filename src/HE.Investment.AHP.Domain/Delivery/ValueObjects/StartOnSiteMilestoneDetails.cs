namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteMilestoneDetails : MilestoneDetails<StartOnSiteDate>
{
    public StartOnSiteMilestoneDetails(StartOnSiteDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static StartOnSiteMilestoneDetails? Create(StartOnSiteDate milestoneDate, MilestonePaymentDate paymentDate)
    {
        return milestoneDate?.Value != null || paymentDate?.Value != null ? new StartOnSiteMilestoneDetails(milestoneDate, paymentDate) : null;
    }
}
