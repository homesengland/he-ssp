using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
internal static class ProjectTestData
{
    public static Project ProjectReadyToBeCompleted()
    {
        var project = new Project();

        project.ChangeName(new ProjectName("name"));

        project.ProvideStartDate(StartDateTestData.CorrectDate);

        project.ProvideHomesCount(HomesCountTestData.ValidHomesCount);

        project.ProvideHomesTypes(new HomesTypes(new string[] { "Home" }, string.Empty));

        project.ProvideProjectType(new ProjectType("Greenfield"));

        project.ProvidePlanningReferenceNumber(PlanningReferenceNumberTestData.ExistingReferenceNumber);

        project.ProvidePlanningPermissionStatus(PlanningPermissionStatusTestData.AnyStatus);

        project.ProvideLandOwnership(new LandOwnership(true));

        project.ProvideAdditionalData(new AdditionalDetails(
            new PurchaseDate(ProjectDateTestData.CorrectDate, ProjectDateTestData.CorrectDateTime),
            PoundsTestData.AnyAmount,
            PoundsTestData.AnyAmount,
            SourceOfValuationTestData.AnySource));

        project.ProvideCoordinates(new Coordinates(LocationTestData.CorrectCoordinates));

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        project.ProvideGrantFundingInformation(PublicSectorGrantFundingTestData.AnyGrantFunding);

        project.ProvideAffordableHomes(new AffordableHomes(CommonResponse.Yes));

        project.ProvideChargesDebt(new ChargesDebt(false, null));

        return project;
    }

    public static Project ProjectWithoutAlternativeData()
    {
        var project = new Project();

        project.ChangeName(new ProjectName("name"));

        project.ProvideStartDate(StartDateTestData.CorrectDate);

        project.ProvideHomesCount(HomesCountTestData.ValidHomesCount);

        project.ProvideHomesTypes(new HomesTypes(new string[] { "Home" }, string.Empty));

        project.ProvideProjectType(new ProjectType("Greenfield"));

        project.ProvidePlanningReferenceNumber(PlanningReferenceNumberTestData.NonExistingReferenceNumber);

        project.ProvideLandOwnership(new LandOwnership(false));

        project.ProvideCoordinates(new Coordinates(LocationTestData.CorrectCoordinates));

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.NotReceived);

        project.ProvideAffordableHomes(new AffordableHomes(CommonResponse.Yes));

        project.ProvideChargesDebt(new ChargesDebt(false, null));

        return project;
    }
}
