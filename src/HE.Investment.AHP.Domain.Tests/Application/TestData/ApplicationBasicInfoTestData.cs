using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestData;

namespace HE.Investment.AHP.Domain.Tests.Application.TestData;

public static class ApplicationBasicInfoTestData
{
    public static readonly ApplicationBasicInfo AffordableRentInDraftState = new(
        AhpApplicationId.From(GuidTestData.GuidTwo),
        new ApplicationName(GuidTestData.GuidTwo.ToString()),
        Tenure.AffordableRent,
        ApplicationStatus.Draft,
        new AhpProgramme(DateTimeTestData.OctoberDay05Year2023At0858, DateTimeTestData.OctoberDay05Year2023At0858.AddMonths(6), MilestoneFramework.Default));

    public static readonly ApplicationBasicInfo SharedOwnershipInDraftState = new(
        AhpApplicationId.From(GuidTestData.GuidOne),
        new ApplicationName(GuidTestData.GuidOne.ToString()),
        Tenure.SharedOwnership,
        ApplicationStatus.Draft,
        new AhpProgramme(DateTimeTestData.OctoberDay05Year2023At0858, DateTimeTestData.OctoberDay05Year2023At0858.AddMonths(6), MilestoneFramework.Default));
}
