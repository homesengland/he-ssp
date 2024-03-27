using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;

public class SiteData
{
    private readonly int _dataSeed;

    public SiteData()
    {
        _dataSeed = new Random().Next(1, 50) * 2;
    }

    public string Id { get; private set; }

    public string Name { get; private set; }

    public string LocalAuthorityName => "Oxford";

    public int HomesNumber => _dataSeed + 4;

    public SitePlanningStatus PlanningStatus => SitePlanningStatus.NoPlanningRequired;

    public SitePlanningStatus NewPlanningStatus => SitePlanningStatus.DetailedPlanningApprovalGranted;

    public bool AddAnotherSite => false;

    public string LocalAuthorityCode(string? useHeTablesParameter) => string.IsNullOrWhiteSpace(useHeTablesParameter) ? "E07000178" : "7000178";

    public string GenerateSiteName()
    {
        Name = "IT-Site".WithTimestampSuffix();
        return Name;
    }

    public void SetId(string id)
    {
        Id = id;
    }
}
