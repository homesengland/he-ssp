using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class SchemeInformationData
{
    private readonly int _dataSeed;

    public SchemeInformationData()
    {
        _dataSeed = new Random().Next(1, 50) * 10;
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
        Affordability = nameof(Affordability).WithTimestampPrefix();
    }

    public void GenerateSalesRisk()
    {
        SalesRisk = nameof(SalesRisk).WithTimestampPrefix();
    }

    public void GenerateHousingNeeds()
    {
        HousingNeedsMeetingLocalPriorities = nameof(HousingNeedsMeetingLocalPriorities).WithTimestampPrefix();
        HousingNeedsMeetingLocalHousingNeed = nameof(HousingNeedsMeetingLocalHousingNeed).WithTimestampPrefix();
    }

    public void GenerateStakeholderDiscussions()
    {
        StakeholderDiscussions = nameof(StakeholderDiscussions).WithTimestampPrefix();
    }

    private decimal GetDecimalValue(string fieldName)
    {
        return _dataSeed + 100_000 + (fieldName.Length * 1_000);
    }
}
