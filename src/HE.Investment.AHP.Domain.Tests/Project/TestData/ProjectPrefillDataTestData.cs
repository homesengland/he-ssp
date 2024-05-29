using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class ProjectPrefillDataTestData
{
    public static ProjectPrefillData FirstProjectPrefillData => new(
        FrontDoorProjectId.From("00000000-0000-1111-1111-111111111113"),
        "First project",
        [SupportActivityType.AcquiringLand, SupportActivityType.SellingLandToHomesEngland],
        [SitePrefillDataTestData.FirstSitePrefillData, SitePrefillDataTestData.SecondSitePrefillData]);
}
