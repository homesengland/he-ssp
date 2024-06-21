using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestoneDetails<T> : ValueObject, IQuestion
    where T : DateValueObject
{
    protected MilestoneDetails(T? milestoneDate, MilestonePaymentDate? paymentDate, string milestoneNameLowercase)
    {
        if (milestoneDate != null &&
            paymentDate != null &&
            paymentDate.IsBefore(milestoneDate))
        {
            throw new DomainValidationException("ClaimMilestonePaymentAt", $"The {milestoneNameLowercase} date must be before, or the same as, the forecast {milestoneNameLowercase} claim date");
        }

        MilestoneDate = milestoneDate;
        PaymentDate = paymentDate;
    }

    public T? MilestoneDate { get; }

    public MilestonePaymentDate? PaymentDate { get; }

    public bool IsAnswered()
    {
        return (MilestoneDate?.IsDateSet ?? false) && (PaymentDate?.IsDateSet ?? false);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return MilestoneDate;
        yield return PaymentDate;
    }
}
