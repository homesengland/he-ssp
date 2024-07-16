using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.WWW.Views.AllocationClaims.Const;

public static class ClaimPageTitles
{
    public const string Summary = "Manage claims";

    public const string Confirmation = "Confirm the following";

    public const string CheckAnswers = "Check your answers before submitting your claim";

    public static string MilestoneOverview(string phaseName) => $"{phaseName} claims";

    public static string CostsIncurred(string phaseName, MilestoneType milestoneType) => $"{phaseName} - {milestoneType.GetDescription()} milestone";

    public static string MilestoneDate(string phaseName, MilestoneType milestoneType) => $"{phaseName} - {milestoneType.GetDescription()} milestone";
}
