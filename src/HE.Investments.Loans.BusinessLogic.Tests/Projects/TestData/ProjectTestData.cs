extern alias Org;

using HE.Investments.Common.Contract.Constants;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;

internal static class ProjectTestData
{
    public static Project ProjectReadyToBeCompleted()
    {
        var project = new Project();

        project.ChangeName(new ProjectName("name"));

        project.ProvideStartDate(StartDateTestData.CorrectDate);

        project.ProvideHomesCount(HomesCountTestData.ValidHomesCount);

        project.ProvideHomesTypes(new HomesTypes(["Home"], string.Empty));

        project.ProvideProjectType(new ProjectType("Greenfield"));

        project.ProvidePlanningReferenceNumber(PlanningReferenceNumberTestData.ExistingReferenceNumber);

        project.ProvidePlanningPermissionStatus(PlanningPermissionStatusTestData.AnyStatus);

        project.ProvideLandOwnership(new LandOwnership(true));

        project.ProvideAdditionalData(new AdditionalDetails(
            new PurchaseDate(ProjectDateTestData.CorrectDateTime),
            PoundsTestData.AnyAmount,
            PoundsTestData.AnyAmount,
            SourceOfValuationTestData.AnySource));

        project.ProvideCoordinates(new Coordinates(LocationTestData.CorrectCoordinates));

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        project.ProvideGrantFundingInformation(PublicSectorGrantFundingTestData.AnyGrantFunding);

        project.ProvideAffordableHomes(new AffordableHomes(CommonResponse.Yes));

        project.ProvideChargesDebt(new ChargesDebt(false, null));

        project.ProvideLocalAuthority(LocalAuthority.New("1", "Liverpool"));

        return project;
    }

    public static Project ProjectWithoutAlternativeData()
    {
        var project = new Project();

        project.ChangeName(new ProjectName("name"));

        project.ProvideStartDate(StartDateTestData.CorrectDate);

        project.ProvideHomesCount(HomesCountTestData.ValidHomesCount);

        project.ProvideHomesTypes(new HomesTypes(["Home"], string.Empty));

        project.ProvideProjectType(new ProjectType("Greenfield"));

        project.ProvidePlanningReferenceNumber(PlanningReferenceNumberTestData.NonExistingReferenceNumber);

        project.ProvideLandOwnership(new LandOwnership(false));

        project.ProvideCoordinates(new Coordinates(LocationTestData.CorrectCoordinates));

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.NotReceived);

        project.ProvideAffordableHomes(new AffordableHomes(CommonResponse.Yes));

        project.ProvideChargesDebt(new ChargesDebt(false, null));

        project.ProvideLocalAuthority(LocalAuthority.New("1", "Liverpool"));

        return project;
    }
}
