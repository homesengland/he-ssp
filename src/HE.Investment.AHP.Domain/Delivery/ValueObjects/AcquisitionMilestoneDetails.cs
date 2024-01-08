using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class AcquisitionMilestoneDetails : ValueObject, IQuestion
{
    public AcquisitionMilestoneDetails(AcquisitionDate? acquisitionDate, MilestonePaymentDate? paymentDate)
    {
        AcquisitionDate = acquisitionDate;
        PaymentDate = paymentDate;
    }

    public AcquisitionDate? AcquisitionDate { get; }

    public MilestonePaymentDate? PaymentDate { get; }

    public static AcquisitionMilestoneDetails? Create(AcquisitionDate? acquisitionDate, MilestonePaymentDate? paymentDate)
    {
        return acquisitionDate.IsProvided() || paymentDate.IsProvided() ? new AcquisitionMilestoneDetails(acquisitionDate, paymentDate) : null;
    }

    public bool IsAnswered()
    {
        return AcquisitionDate.IsProvided() && PaymentDate.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return AcquisitionDate;
        yield return PaymentDate;
    }
}
