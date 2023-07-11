using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects;

[TestClass]
public class AllDataIsProvidedTests
{
    private SiteViewModel _model;

    [TestInitialize]
    public void Init()
    {
        _model = SiteViewModelObjectBuilder
            .NewObject()
            .ThatPassesCheckAnswersValidation()
            .Build();
    }

    [TestMethod]
    public void AllDataIsProvided()
    {
        _model.AllInformationIsProvided().Should().BeTrue();
    }

    [TestMethod]
    public void FailWhenBasicDataIsNotProvided()
    {
        _model.Name = string.Empty;
        _model.ManyHomes = string.Empty;
        _model.TypeHomes = Array.Empty<string>();
        _model.Type = string.Empty;
        _model.AffordableHomes = string.Empty;
        _model.ChargesDebt = string.Empty;
        _model.HomesToBuild = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailReferenceNumberIsNotProvided()
    {
        _model.PlanningRef = null;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsNoAndNoLocationWasProvided()
    {
        _model.PlanningRef = "No";
        _model.LocationOption = "coordinates";
        _model.LocationCoordinates = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsNoAndLandRegistryIsNotProvided()
    {
        _model.PlanningRef = "No";
        _model.LocationOption = "landRegistryTitleNumber";
        _model.LocationLandRegistry = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsYesAndReferenceNumberIsNotProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsYesAndPlanningStatusIsNotProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = "number";
        _model.PlanningStatus = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsYesAndLocationIsNotProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = "number";
        _model.PlanningStatus = "status";

        _model.LocationOption = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsYesAndCoordinatesAreNotProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = "number";
        _model.PlanningStatus = "status";

        _model.LocationOption = "coordinates";
        _model.LocationCoordinates = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenPlanningRefIsYesAndLandRegistryIsNotProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = "number";
        _model.PlanningStatus = "status";

        _model.LocationOption = "landRegistryTitleNumber";
        _model.LocationCoordinates = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void PassWhenAllPlanningRefDataIsProvided()
    {
        _model.PlanningRef = "Yes";
        _model.PlanningRefEnter = "number";
        _model.PlanningStatus = "status";

        _model.AllInformationIsProvided().Should().BeTrue();
    }

    [TestMethod]
    public void FailWhenOwnershipIsNotProvided()
    {
        _model.Ownership = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenOwnershipIsYesAndAdditionalDataIsNotProvided()
    {
        _model.Ownership = "Yes";

        _model.PurchaseDate = null;
        _model.Cost = string.Empty;
        _model.Value = string.Empty;
        _model.Type = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void PassWhenOwnershipIsYesAndAdditionalDataIsProvided()
    {
        _model.Ownership = "Yes";

        _model.PurchaseDate = new DateTime(2023, 7, 7);
        _model.Cost = "123";
        _model.Value = "123";
        _model.Type = "type";

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void PassWhenOwnershipIsNoAndAdditionalDataIsNotProvided()
    {
        _model.Ownership = "No";

        _model.PurchaseDate = null;
        _model.Cost = string.Empty;
        _model.Value = string.Empty;
        _model.Source = string.Empty;

        _model.AllInformationIsProvided().Should().BeTrue();
    }

    [TestMethod]
    public void FailWhenGrantFundingIsNotProvided()
    {
        _model.GrantFunding = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void FailWhenGrantFundingIsYesAndNoAdditionalInformationIsProvided()
    {
        _model.GrantFunding = "Yes";

        _model.GrantFundingName = string.Empty;
        _model.GrantFundingSource = string.Empty;
        _model.GrantFundingAmount = string.Empty;
        _model.GrantFundingPurpose = string.Empty;

        _model.AllInformationIsProvided().Should().BeFalse();
    }

    [TestMethod]
    public void PassWhenGrantFundingIsYesAndAllAdditionalInformationIsProvided()
    {
        _model.GrantFunding = "Yes";

        _model.GrantFundingName = "name";
        _model.GrantFundingSource = "source";
        _model.GrantFundingAmount = "12";
        _model.GrantFundingPurpose = "purpose";

        _model.AllInformationIsProvided().Should().BeTrue();
    }

    [TestMethod]
    public void PassWhenGrantFundingIsNoAndNoAdditionalInformationIsProvided()
    {
        _model.GrantFunding = "No";

        _model.GrantFundingName = string.Empty;
        _model.GrantFundingSource = string.Empty;
        _model.GrantFundingAmount = string.Empty;
        _model.GrantFundingPurpose = string.Empty;

        _model.AllInformationIsProvided().Should().BeTrue();
    }
}
