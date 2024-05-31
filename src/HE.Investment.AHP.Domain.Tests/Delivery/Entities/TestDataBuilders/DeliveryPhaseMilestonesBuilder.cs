using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

internal sealed class DeliveryPhaseMilestonesBuilder : TestObjectBuilder<DeliveryPhaseMilestonesBuilder, DeliveryPhaseMilestones>
{
    public DeliveryPhaseMilestonesBuilder()
        : base(new DeliveryPhaseMilestones(false, null, null, null))
    {
    }

    protected override DeliveryPhaseMilestonesBuilder Builder => this;

    public DeliveryPhaseMilestonesBuilder WithIsOnlyCompletionMilestone(bool value) => SetProperty(x => x.IsOnlyCompletionMilestone, value);

    public DeliveryPhaseMilestonesBuilder WithAcquisitionMilestone(AcquisitionMilestoneDetails value) => SetProperty(x => x.AcquisitionMilestone, value);

    public DeliveryPhaseMilestonesBuilder WithStartOnSiteMilestone(StartOnSiteMilestoneDetails value) => SetProperty(x => x.StartOnSiteMilestone, value);

    public DeliveryPhaseMilestonesBuilder WithCompletionMilestone(CompletionMilestoneDetails value) => SetProperty(x => x.CompletionMilestone, value);
}
