using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionMilestoneDetails : MilestoneDetails<CompletionDate>
{
    public CompletionMilestoneDetails(CompletionDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static CompletionMilestoneDetails? Create(CompletionDate milestoneDate, MilestonePaymentDate paymentDate)
    {
        return milestoneDate.Value.HasValue || paymentDate.Value.HasValue ? new CompletionMilestoneDetails(milestoneDate, paymentDate) : null;
    }
}
