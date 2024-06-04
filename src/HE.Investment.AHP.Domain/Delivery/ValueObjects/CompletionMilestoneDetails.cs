namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public sealed class CompletionMilestoneDetails : MilestoneDetails<CompletionDate>
{
    private CompletionMilestoneDetails(CompletionDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static CompletionMilestoneDetails? Create(CompletionDate? milestoneDate, MilestonePaymentDate? paymentDate)
    {
        return milestoneDate?.Value != null || paymentDate?.Value != null ? new CompletionMilestoneDetails(milestoneDate, paymentDate) : null;
    }
}
