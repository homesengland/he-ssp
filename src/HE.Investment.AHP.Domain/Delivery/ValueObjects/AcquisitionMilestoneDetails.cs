namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public sealed class AcquisitionMilestoneDetails : MilestoneDetails<AcquisitionDate>
{
    private AcquisitionMilestoneDetails(AcquisitionDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static AcquisitionMilestoneDetails? Create(AcquisitionDate? acquisitionDate, MilestonePaymentDate? paymentDate)
    {
        return acquisitionDate?.Value != null || paymentDate?.Value != null ? new AcquisitionMilestoneDetails(acquisitionDate, paymentDate) : null;
    }
}
