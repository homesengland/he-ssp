using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Tests.TestData;
using HE.Investments.Common.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Tests.Application.TestData;

public static class ApplicationBasicInfoTestData
{
    public static readonly ApplicationBasicInfo AffordableRentInDraftState = new(
        ApplicationId.From(GuidTestData.GuidTwo),
        new ApplicationName(GuidTestData.GuidTwo.ToString()),
        Tenure.AffordableRent,
        ApplicationStatus.Draft);

    public static readonly ApplicationBasicInfo SharedOwnershipInDraftState = new(
        ApplicationId.From(GuidTestData.GuidOne),
        new ApplicationName(GuidTestData.GuidOne.ToString()),
        Tenure.SharedOwnership,
        ApplicationStatus.Draft);
}
