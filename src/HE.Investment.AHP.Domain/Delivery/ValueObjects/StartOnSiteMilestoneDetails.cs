using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteMilestoneDetails : MilestoneDetails<StartOnSiteDate>
{
    public StartOnSiteMilestoneDetails(StartOnSiteDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static StartOnSiteMilestoneDetails? Create(StartOnSiteDate milestoneDate, MilestonePaymentDate paymentDate)
    {
        return milestoneDate.Value.HasValue || paymentDate.Value.HasValue ? new StartOnSiteMilestoneDetails(milestoneDate, paymentDate) : null;
    }
}
