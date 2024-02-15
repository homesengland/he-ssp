using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Programme.TestData;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestData;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class DeliveryPhaseEntityBuilder
{
    private readonly IList<HomesToDeliverInPhase> _homesToDeliver = new List<HomesToDeliverInPhase>();

    private string _id = "dp-1-12313";

    private ApplicationBasicInfo _applicationBasicInfo = new(
        new AhpApplicationId("test-app-42123"),
        new ApplicationName("Test Application"),
        Tenure.AffordableRent,
        ApplicationStatus.Draft,
        new AhpProgramme(ProgrammeDatesTestData.ProgrammeDates, MilestoneFramework.Default));

    private DeliveryPhaseName _name = new("First Phase");

    private SectionStatus _status = SectionStatus.InProgress;

    private OrganisationBasicInfo _organisationBasicInfo = new OrganisationBasicInfoBuilder().Build();

    private IsAdditionalPaymentRequested? _isAdditionalPaymentRequested;

    private TypeOfHomes? _typeOfHomes = TypeOfHomes.NewBuild;

    private BuildActivity _buildActivity = new(Tenure.AffordableRent, TypeOfHomes.NewBuild, BuildActivityType.Regeneration);

    private bool? _reconfigureExisting;

    private SchemeFunding _schemeFunding = new(1_000_000, 15);

    private AcquisitionMilestoneDetails? _acquisitionMilestone = new AcquisitionMilestoneDetailsBuilder().Build();

    private StartOnSiteMilestoneDetails? _startOnSiteMilestone = new StartOnSiteMilestoneDetailsBuilder().Build();

    private CompletionMilestoneDetails? _completionMilestone = new CompletionMilestoneDetailsBuilder().Build();

    public DeliveryPhaseEntityBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithName(string name)
    {
        _name = new DeliveryPhaseName(name);
        return this;
    }

    public DeliveryPhaseEntityBuilder WithHomesToBeDelivered(int numberOfHomes, string? homeTypeId = null)
    {
        _homesToDeliver.Add(new HomesToDeliverInPhase(new HomeTypeId(homeTypeId ?? $"ht-{_homesToDeliver.Count + 1}"), numberOfHomes));
        return this;
    }

    public DeliveryPhaseEntityBuilder WithHomesToBeDelivered()
    {
        _homesToDeliver.Add(new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 1));
        return this;
    }

    public DeliveryPhaseEntityBuilder WithStatus(SectionStatus status)
    {
        _status = status;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithUnregisteredBody()
    {
        _organisationBasicInfo = new OrganisationBasicInfoBuilder().WithUnregisteredBody().Build();
        return this;
    }

    public DeliveryPhaseEntityBuilder WithAdditionalPaymentRequested(IsAdditionalPaymentRequested isAdditionalPaymentRequested)
    {
        _isAdditionalPaymentRequested = isAdditionalPaymentRequested;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutBuildActivity()
    {
        _buildActivity = new BuildActivity(_applicationBasicInfo.Tenure);
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutTypeOfHomes()
    {
        _typeOfHomes = null;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithTypeOfHomes(TypeOfHomes? typeOfHomes)
    {
        _typeOfHomes = typeOfHomes;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutHomesToDeliver()
    {
        _homesToDeliver.Clear();

        return this;
    }

    public DeliveryPhaseEntityBuilder WithReconfiguringExisting()
    {
        _reconfigureExisting = true;

        return this;
    }

    public DeliveryPhaseEntityBuilder WithRehabBuildActivity(BuildActivityType buildActivityType = BuildActivityType.ExistingSatisfactory)
    {
        _buildActivity = new BuildActivity(_applicationBasicInfo.Tenure, TypeOfHomes.Rehab, buildActivityType);

        return this;
    }

    public DeliveryPhaseEntityBuilder WithNewBuildActivity(BuildActivityType buildActivityType = BuildActivityType.AcquisitionAndWorks)
    {
        _buildActivity = new BuildActivity(_applicationBasicInfo.Tenure, TypeOfHomes.NewBuild, buildActivityType);

        return this;
    }

    public DeliveryPhaseEntityBuilder WithSchemeFunding(int? requiredFunding, int? homesToDeliver)
    {
        _schemeFunding = new SchemeFunding(requiredFunding, homesToDeliver);
        return this;
    }

    public DeliveryPhaseEntityBuilder WithAcquisitionMilestone(AcquisitionMilestoneDetails acquisitionMilestone)
    {
        _acquisitionMilestone = acquisitionMilestone;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutAcquisitionMilestone()
    {
        _acquisitionMilestone = null;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithStartOnSiteMilestone(StartOnSiteMilestoneDetails startOnSiteMilestone)
    {
        _startOnSiteMilestone = startOnSiteMilestone;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutStartOnSiteMilestone()
    {
        _startOnSiteMilestone = null;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithCompletionMilestone(CompletionMilestoneDetails completionMilestone)
    {
        _completionMilestone = completionMilestone;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutCompletionMilestone()
    {
        _completionMilestone = null;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithMilestoneFramework(MilestoneFramework milestoneFramework)
    {
        _applicationBasicInfo = _applicationBasicInfo with
        {
            Programme = new AhpProgramme(
                ProgrammeDatesTestData.ProgrammeDates,
                milestoneFramework),
        };

        return this;
    }

    public DeliveryPhaseEntity Build()
    {
        return new DeliveryPhaseEntity(
            _applicationBasicInfo,
            _name,
            _organisationBasicInfo,
            _status,
            MilestonesPercentageTranches.LackOfCalculation,
            _schemeFunding,
            _typeOfHomes,
            _buildActivity,
            _reconfigureExisting,
            _homesToDeliver,
            _acquisitionMilestone,
            _startOnSiteMilestone,
            _completionMilestone,
            new DeliveryPhaseId(_id),
            DateTimeTestData.OctoberDay05Year2023At0858,
            isAdditionalPaymentRequested: _isAdditionalPaymentRequested);
    }
}
