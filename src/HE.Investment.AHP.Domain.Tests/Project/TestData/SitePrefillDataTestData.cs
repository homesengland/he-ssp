using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class SitePrefillDataTestData
{
    public static readonly SitePrefillData FirstSitePrefillData = new(
        FrontDoorSiteId.From("00000000-0000-1111-1111-111111111111"),
        "First site",
        10,
        SitePlanningStatus.DetailedPlanningApprovalGranted,
        "Liverpool");

    public static readonly SitePrefillData SecondSitePrefillData = new(
        FrontDoorSiteId.From("00000000-0000-2222-2222-111111111112"),
        "Second site",
        20,
        SitePlanningStatus.OutlinePlanningApplicationSubmitted,
        "Liverpool");
}
