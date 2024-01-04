using System.Globalization;
using System.Security.Principal;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class SchemaInformationData
{
    public decimal? RequiredFunding { get; private set; }

    public int? HousesToDeliver { get; private set; }

    public string Affordability { get; private set; }

    public string SalesRisk { get; private set; }

    public string HousingNeedsMeetingLocalPriorities { get; private set; }

    public string HousingNeedsMeetingLocalHousingNeed { get; private set; }

    public string StakeholderDiscussions { get; private set; }

    public void GenerateFundingDetails()
    {
        RequiredFunding = DateTime.UtcNow.Millisecond;
        HousesToDeliver = DateTime.UtcNow.Second;
    }

    public void GenerateAffordability()
    {
        Affordability = $"{nameof(Affordability)}_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }

    public void GenerateSalesRisk()
    {
        SalesRisk = $"{nameof(SalesRisk)}_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }

    public void GenerateHousingNeeds()
    {
        HousingNeedsMeetingLocalPriorities = $"{nameof(HousingNeedsMeetingLocalPriorities)}_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        HousingNeedsMeetingLocalHousingNeed = $"{nameof(HousingNeedsMeetingLocalHousingNeed)}_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }

    public void GenerateStakeholderDiscussions()
    {
        StakeholderDiscussions = $"{nameof(StakeholderDiscussions)}_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }
}
