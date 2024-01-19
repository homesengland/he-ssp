using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class DeliveryPhaseMilestonesBuilder
{
    private OrganisationBasicInfo _testOrganisation = new OrganisationBasicInfoBuilder().Build();
    private BuildActivity _buildActivity = new(Tenure.OlderPersonsSharedOwnership);
    private AcquisitionMilestoneDetails? _acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder().Build();
    private StartOnSiteMilestoneDetails? _startOnSiteMilestoneDetails = new StartOnSiteMilestoneDetailsBuilder().Build();
    private CompletionMilestoneDetails? _completionMilestoneDetails = new CompletionMilestoneDetailsBuilder().Build();

    public DeliveryPhaseMilestonesBuilder WithUnregisteredBody()
    {
        _testOrganisation = new OrganisationBasicInfoBuilder().WithUnregisteredBody().Build();
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithoutAcquisitionMilestoneDetails()
    {
        _acquisitionMilestoneDetails = null;
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithAcquisitionMilestoneDetails(AcquisitionMilestoneDetails milestoneDetails)
    {
        _acquisitionMilestoneDetails = milestoneDetails;
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithoutStartOnSiteMilestoneDetails()
    {
        _startOnSiteMilestoneDetails = null;
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithAcquisitionPaymentDateAfterStartOnSitePaymentDate()
    {
        _acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithPaymentDate(MilestonePaymentDateFactory.GetDateDayAfter(_startOnSiteMilestoneDetails?.PaymentDate))
            .Build();
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithAcquisitionPaymentDateAfterCompletionPaymentDate()
    {
        _acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithPaymentDate(MilestonePaymentDateFactory.GetDateDayAfter(_completionMilestoneDetails?.PaymentDate))
            .Build();
        _startOnSiteMilestoneDetails = null;
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithStartOnSitePaymentDateAfterCompletionPaymentDate()
    {
        _startOnSiteMilestoneDetails = new StartOnSiteMilestoneDetailsBuilder()
            .WithPaymentDate(MilestonePaymentDateFactory.GetDateDayAfter(_completionMilestoneDetails?.PaymentDate))
            .Build();
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithoutCompletionMilestoneDetails()
    {
        _completionMilestoneDetails = null;
        return this;
    }

    public DeliveryPhaseMilestonesBuilder WithBuildActivityOnlyForCompletionMilestone()
    {
        _buildActivity = new BuildActivity(Tenure.AffordableRent, TypeOfHomes.Rehab, BuildActivityType.ExistingSatisfactory);
        return this;
    }

    public DeliveryPhaseMilestones Build()
    {
        return new DeliveryPhaseMilestones(
            _testOrganisation,
            _buildActivity,
            _acquisitionMilestoneDetails,
            _startOnSiteMilestoneDetails,
            _completionMilestoneDetails);
    }
}
