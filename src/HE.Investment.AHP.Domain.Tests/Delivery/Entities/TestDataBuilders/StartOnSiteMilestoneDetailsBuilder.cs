using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

internal sealed class StartOnSiteMilestoneDetailsBuilder : TestObjectBuilder<StartOnSiteMilestoneDetailsBuilder, StartOnSiteMilestoneDetails>
{
    public StartOnSiteMilestoneDetailsBuilder()
        : base(Answered)
    {
    }

    public static StartOnSiteMilestoneDetails Answered =>
        StartOnSiteMilestoneDetails.Create(new StartOnSiteDate(true, "11", "1", "2025"), new MilestonePaymentDate(true, "1", "6", "2026"))!;

    protected override StartOnSiteMilestoneDetailsBuilder Builder => this;

    public StartOnSiteMilestoneDetailsBuilder WithMilestoneDate(DateTime value) => SetProperty(x => x.MilestoneDate, new StartOnSiteDate(value));

    public StartOnSiteMilestoneDetailsBuilder WithPaymentDate(DateTime value) => SetProperty(x => x.PaymentDate, new MilestonePaymentDate(value));

    public StartOnSiteMilestoneDetailsBuilder WithoutPaymentDate() => SetProperty(x => x.PaymentDate, null);
}
