using System.Globalization;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class SchemeInformationData
{
    private readonly int _dataSeed;

    public SchemeInformationData()
    {
        _dataSeed = new Random().Next(1, 100);
    }

    public decimal RequiredFunding { get; private set; }

    public int HousesToDeliver { get; private set; }

    public string Affordability { get; private set; }

    public string SalesRisk { get; private set; }

    public string HousingNeedsMeetingLocalPriorities { get; private set; }

    public string HousingNeedsMeetingLocalHousingNeed { get; private set; }

    public string StakeholderDiscussions { get; private set; }

    public void GenerateFundingDetails()
    {
        RequiredFunding = GetDecimalValue(nameof(RequiredFunding));
        HousesToDeliver = _dataSeed;
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

    private decimal GetDecimalValue(string fieldName)
    {
        return _dataSeed + 100_000 + (fieldName.Length * 1_000);
    }
}
