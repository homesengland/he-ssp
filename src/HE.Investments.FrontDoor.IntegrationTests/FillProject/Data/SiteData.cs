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

    public string LocalAuthorityCode => "E09000001";

    public string LocalAuthorityName => "City of London";

    public int HomesNumber => _dataSeed + 4;

    public SitePlanningStatus PlanningStatus => SitePlanningStatus.OutlinePlanningApplicationSubmitted;

    public bool AddAnotherSite => false;

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