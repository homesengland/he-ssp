using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
internal sealed class ProjectTestBuilder
{
    private readonly Project _project;

    public ProjectTestBuilder()
    {
        _project = new Project();
    }

    public static ProjectTestBuilder New() => new();

    public ProjectTestBuilder ReadyToBeCompleted()
    {
        _project.ChangeName(new ProjectName("name"));

        _project.ProvideStartDate(StartDateTestData.CorrectDate);

        _project.ProvideHomesCount(HomesCountTestData.ValidHomesCount);

        _project.ProvideHomesTypes(new HomesTypes(new string[] { "Home" }, string.Empty));

        _project.ProvideProjectType(new ProjectType("Greenfield"));

        _project.ProvidePlanningReferenceNumber(PlanningReferenceNumberTestData.ExistingReferenceNumber);

        _project.ProvidePlanningPermissionStatus(PlanningPermissionStatusTestData.AnyStatus);

        _project.ProvideLandOwnership(new LandOwnership(true));

        _project.ProvideAdditionalData(new AdditionalDetails(
            new PurchaseDate(ProjectDateTestData.CorrectDate, ProjectDateTestData.CorrectDateTime),
            PoundsTestData.AnyAmount,
            PoundsTestData.AnyAmount,
            SourceOfValuationTestData.AnySource));

        _project.ProvideCoordinates(new Coordinates(LocationTestData.CorrectCoordinates));

        _project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        _project.ProvideGrantFundingInformation(PublicSectorGrantFundingTestData.AnyGrantFunding);

        _project.ProvideAffordableHomes(new AffordableHomes(CommonResponse.Yes));

        _project.ProvideChargesDebt(new ChargesDebt(false, null));

        return this;
    }

    public Project Build()
    {
        return _project;
    }
}
