using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;

public class ProjectData
{
    private readonly int _dataSeed;

    public ProjectData()
    {
        _dataSeed = new Random().Next(1, 50) * 2;
        SiteData = new SiteData();
    }

    public string Id { get; private set; }

    public string Name { get; private set; }

    public SupportActivityType ActivityType => SupportActivityType.DevelopingHomes;

    public bool IsEnglandHousingDelivery => true;

    public AffordableHomesAmount AffordableHomeAmount => AffordableHomesAmount.OnlyAffordableHomes;

    public ProjectGeographicFocus GeographicFocus => ProjectGeographicFocus.Regional;

    public IList<RegionType> RegionTypes => new List<RegionType> { RegionType.NorthEast, RegionType.London };

    public int OrganisationHomesBuilt => _dataSeed + 2000;

    public int HomesNumber => _dataSeed + 2;

    public bool IsSiteIdentified { get; set; }

    public bool IsSupportRequired => true;

    public bool IsFundingRequired { get; set; }

    public RequiredFundingOption RequiredFunding => RequiredFundingOption.Between5MlnAnd10Mln;

    public bool IsProfit => true;

    public SiteData SiteData { get; }

    public DateTime ExpectedStartDate => DateTime.UtcNow.AddDays(_dataSeed);

    public void SetProjectId(string projectId)
    {
        Id = projectId;
    }

    public string GenerateProjectName()
    {
        Name = "IT-Project".WithTimestampSuffix();
        return Name;
    }

    public void SwitchIsSiteIdentified()
    {
        IsSiteIdentified = !IsSiteIdentified;
        IsFundingRequired = true;
    }
}
