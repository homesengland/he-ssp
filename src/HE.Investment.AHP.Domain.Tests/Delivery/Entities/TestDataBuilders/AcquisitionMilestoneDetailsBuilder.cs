using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

internal sealed class AcquisitionMilestoneDetailsBuilder : TestObjectBuilder<AcquisitionMilestoneDetailsBuilder, AcquisitionMilestoneDetails>
{
    public AcquisitionMilestoneDetailsBuilder()
        : base(Answered)
    {
    }

    public static AcquisitionMilestoneDetails Answered =>
        AcquisitionMilestoneDetails.Create(new AcquisitionDate(true, "10", "1", "2025"), new MilestonePaymentDate(true, "1", "5", "2026"))!;

    protected override AcquisitionMilestoneDetailsBuilder Builder => this;

    public AcquisitionMilestoneDetailsBuilder WithMilestoneDate(DateTime value) => SetProperty(x => x.MilestoneDate, new AcquisitionDate(value));

    public AcquisitionMilestoneDetailsBuilder WithPaymentDate(DateTime value) => SetProperty(x => x.PaymentDate, new MilestonePaymentDate(value));

    public AcquisitionMilestoneDetailsBuilder WithoutPaymentDate() => SetProperty(x => x.PaymentDate, null);
}
