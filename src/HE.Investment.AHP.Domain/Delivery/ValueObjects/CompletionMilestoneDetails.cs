using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionMilestoneDetails : ValueObject, IQuestion
{
    public CompletionMilestoneDetails(CompletionDate? completionDate, MilestonePaymentDate? paymentDate)
    {
        CompletionDate = completionDate;
        PaymentDate = paymentDate;
    }

    public CompletionDate? CompletionDate { get; }

    public MilestonePaymentDate? PaymentDate { get; }

    public static CompletionMilestoneDetails? Create(CompletionDate? completionDate, MilestonePaymentDate? paymentDate)
    {
        return completionDate.IsProvided() || paymentDate.IsProvided() ? new CompletionMilestoneDetails(completionDate, paymentDate) : null;
    }

    public bool IsAnswered()
    {
        return CompletionDate.IsProvided() && PaymentDate.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CompletionDate;
        yield return PaymentDate;
    }
}
