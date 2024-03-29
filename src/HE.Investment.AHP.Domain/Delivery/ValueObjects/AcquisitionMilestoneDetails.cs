using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class AcquisitionMilestoneDetails : MilestoneDetails<AcquisitionDate>
{
    public AcquisitionMilestoneDetails(AcquisitionDate? milestoneDate, MilestonePaymentDate? paymentDate)
        : base(milestoneDate, paymentDate)
    {
    }

    public static AcquisitionMilestoneDetails? Create(AcquisitionDate? acquisitionDate, MilestonePaymentDate? paymentDate)
    {
        return acquisitionDate.IsProvided() || paymentDate.IsProvided() ? new AcquisitionMilestoneDetails(acquisitionDate, paymentDate) : null;
    }
}
