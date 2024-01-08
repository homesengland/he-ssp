using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.Site.ValueObjects;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestData;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class DeliveryPhaseEntityBuilder
{
    private readonly IList<HomesToDeliverInPhase> _homesToDeliver = new List<HomesToDeliverInPhase>();

    private string _id = "dp-1-12313";

    private SectionStatus _status = SectionStatus.InProgress;

    private SiteBasicInfo _siteBasicInfo = new(new SiteId("S1"), false);

    private AcquisitionMilestoneDetails? _acquisitionMilestoneDetails;

    private StartOnSiteMilestoneDetails? _startOnSiteMilestoneDetails;

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

    public DeliveryPhaseEntityBuilder WithStatus(SectionStatus status)
    {
        _status = status;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithUnregisteredBody()
    {
        _siteBasicInfo = new(new SiteId("S1"), true);
        return this;
    }

    public DeliveryPhaseEntityBuilder WithAcquisitionMilestoneDetails(AcquisitionMilestoneDetails milestoneDetails)
    {
        _acquisitionMilestoneDetails = milestoneDetails;
        return this;
    }

    public DeliveryPhaseEntityBuilder WithStartOnSiteMilestoneDetails(StartOnSiteMilestoneDetails milestoneDetails)
    {
        _startOnSiteMilestoneDetails = milestoneDetails;
        return this;
    }

    public DeliveryPhaseEntity Build()
    {
        return new DeliveryPhaseEntity(
            new ApplicationBasicInfo(
                new ApplicationId("test-app-42123"),
                new ApplicationName("Test Application"),
                Tenure.AffordableRent,
                ApplicationStatus.Draft),
            _siteBasicInfo,
            "First Phase",
            TypeOfHomes.Rehab,
            _status,
            _homesToDeliver,
            new DeliveryPhaseId(_id),
            DateTimeTestData.OctoberDay05Year2023At0858,
            acquisitionMilestone: _acquisitionMilestoneDetails,
            startOnSiteMilestone: _startOnSiteMilestoneDetails);
    }
}
