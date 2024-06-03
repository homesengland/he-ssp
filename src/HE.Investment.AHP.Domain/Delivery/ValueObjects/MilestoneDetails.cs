using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestoneDetails<T> : ValueObject, IQuestion
    where T : DateValueObject
{
    protected MilestoneDetails(T? milestoneDate, MilestonePaymentDate? paymentDate)
    {
        if (milestoneDate != null &&
            paymentDate != null &&
            paymentDate.IsBefore(milestoneDate))
        {
            throw new DomainValidationException("ClaimMilestonePaymentAt", "The milestone payment date must be on or after the milestone date");
        }

        MilestoneDate = milestoneDate;
        PaymentDate = paymentDate;
    }

    public T? MilestoneDate { get; }

    public MilestonePaymentDate? PaymentDate { get; }

    public bool IsAnswered()
    {
        return MilestoneDate.IsProvided() && PaymentDate.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return MilestoneDate;
        yield return PaymentDate;
    }
}
