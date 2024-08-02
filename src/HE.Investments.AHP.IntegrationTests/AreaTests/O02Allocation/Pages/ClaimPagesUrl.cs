using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;

public sealed class ClaimPagesUrl
{
    private readonly string _claimPageUrl;

    public ClaimPagesUrl(string organisationId, string allocationId, string phaseId, MilestoneType milestoneType)
    {
        _claimPageUrl = $"ahp/{ShortGuid.FromString(organisationId).Value}" +
                        $"/allocation/{ShortGuid.FromString(allocationId).Value}" +
                        $"/claims/{ShortGuid.FromString(phaseId).Value}/{milestoneType}";
    }

    public string CostsIncurred => $"{_claimPageUrl}/costs-incurred";

    public string AchievementDate => $"{_claimPageUrl}/achievement-date";

    public string Confirmation => $"{_claimPageUrl}/confirmation";

    public string CheckAnswers => $"{_claimPageUrl}/check-answers";
}
