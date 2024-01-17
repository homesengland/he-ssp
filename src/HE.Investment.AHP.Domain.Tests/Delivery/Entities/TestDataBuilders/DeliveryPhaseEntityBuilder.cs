using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestData;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class DeliveryPhaseEntityBuilder
{
    private static readonly ApplicationBasicInfo ApplicationBasicInfo = new(
        new AhpApplicationId("test-app-42123"),
        new ApplicationName("Test Application"),
        Tenure.AffordableRent,
        ApplicationStatus.Draft);

    private readonly IList<HomesToDeliverInPhase> _homesToDeliver = new List<HomesToDeliverInPhase>();

    private string _id = "dp-1-12313";

    private SectionStatus _status = SectionStatus.InProgress;

    private OrganisationBasicInfo _organisationBasicInfo = new OrganisationBasicInfoBuilder().Build();

    private DeliveryPhaseMilestones? _deliveryPhaseMilestones = new DeliveryPhaseMilestonesBuilder().Build();

    private IsAdditionalPaymentRequested? _isAdditionalPaymentRequested;

    private TypeOfHomes? _typeOfHomes = TypeOfHomes.NewBuild;

    private BuildActivity _buildActivity = new(ApplicationBasicInfo.Tenure, TypeOfHomes.NewBuild, BuildActivityType.Regeneration);

    private bool? _reconfigureExisting;

    public DeliveryPhaseEntityBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithHomesToBeDelivered(string homeTypeId, int numberOfHomes)
    {
        _homesToDeliver.Add(new HomesToDeliverInPhase(new HomeTypeId(homeTypeId), numberOfHomes));
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

    public DeliveryPhaseEntityBuilder WithDeliveryPhaseMilestones(DeliveryPhaseMilestones milestones)
    {
        _deliveryPhaseMilestones = milestones;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithAdditionalPaymentRequested(IsAdditionalPaymentRequested isAdditionalPaymentRequested)
    {
        _isAdditionalPaymentRequested = isAdditionalPaymentRequested;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutBuildActivity()
    {
        _buildActivity = new BuildActivity(ApplicationBasicInfo.Tenure);
        return this;
    }

    public DeliveryPhaseEntityBuilder WithoutTypeOfHomes()
    {
        _typeOfHomes = null;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithTypeOfHomes(TypeOfHomes typeOfHomes)
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

    public DeliveryPhaseEntity Build()
    {
        return new DeliveryPhaseEntity(
            ApplicationBasicInfo,
            new DeliveryPhaseName("First Phase"),
            _organisationBasicInfo,
            _status,
            _typeOfHomes,
            _buildActivity,
            _reconfigureExisting,
            _homesToDeliver,
            _deliveryPhaseMilestones ?? new DeliveryPhaseMilestonesBuilder().Build(),
            new DeliveryPhaseId(_id),
            DateTimeTestData.OctoberDay05Year2023At0858,
            isAdditionalPaymentRequested: _isAdditionalPaymentRequested);
    }
}
