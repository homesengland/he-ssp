using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investment.AHP.Domain.Tests.Application.TestData;

public static class ApplicationBasicInfoTestData
{
    public static readonly ApplicationBasicInfo AffordableRentInDraftState = new(
        AhpApplicationId.From(GuidTestData.GuidTwo),
        new ApplicationName(GuidTestData.GuidTwo.ToString()),
        Tenure.AffordableRent,
        ApplicationStatus.Draft);

    public static readonly ApplicationBasicInfo SharedOwnershipInDraftState = new(
        AhpApplicationId.From(GuidTestData.GuidOne),
        new ApplicationName(GuidTestData.GuidOne.ToString()),
        Tenure.SharedOwnership,
        ApplicationStatus.Draft);
}
