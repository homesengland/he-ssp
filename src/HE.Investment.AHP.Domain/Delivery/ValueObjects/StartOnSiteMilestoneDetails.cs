using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteMilestoneDetails : ValueObject, IQuestion
{
    public StartOnSiteMilestoneDetails(StartOnSiteDate? startOnSiteDate, MilestonePaymentDate? paymentDate)
    {
        StartOnSiteDate = startOnSiteDate;
        PaymentDate = paymentDate;
    }

    public StartOnSiteDate? StartOnSiteDate { get; }

    public MilestonePaymentDate? PaymentDate { get; }

    public static StartOnSiteMilestoneDetails? Create(StartOnSiteDate? startOnSiteDate, MilestonePaymentDate? paymentDate)
    {
        return startOnSiteDate.IsProvided() || paymentDate.IsProvided() ? new StartOnSiteMilestoneDetails(startOnSiteDate, paymentDate) : null;
    }

    public bool IsAnswered()
    {
        return StartOnSiteDate.IsProvided() && PaymentDate.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartOnSiteDate;
        yield return PaymentDate;
    }
}
