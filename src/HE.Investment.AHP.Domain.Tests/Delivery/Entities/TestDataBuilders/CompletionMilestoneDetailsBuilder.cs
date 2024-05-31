using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

internal sealed class CompletionMilestoneDetailsBuilder : TestObjectBuilder<CompletionMilestoneDetailsBuilder, CompletionMilestoneDetails>
{
    public CompletionMilestoneDetailsBuilder()
        : base(Answered)
    {
    }

    public static CompletionMilestoneDetails Answered =>
        CompletionMilestoneDetails.Create(new CompletionDate(true, "12", "1", "2025"), new MilestonePaymentDate(true, "1", "7", "2026"))!;

    protected override CompletionMilestoneDetailsBuilder Builder => this;

    public CompletionMilestoneDetailsBuilder WithMilestoneDate(DateTime value) => SetProperty(x => x.MilestoneDate, new CompletionDate(value));

    public CompletionMilestoneDetailsBuilder WithPaymentDate(DateTime value) => SetProperty(x => x.PaymentDate, new MilestonePaymentDate(value));

    public CompletionMilestoneDetailsBuilder WithoutPaymentDate() => SetProperty(x => x.PaymentDate, null);
}
