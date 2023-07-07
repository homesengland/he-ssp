using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.ViewModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects
{
    [TestClass]
    public class AllDataIsProvidedTests
    {
        private SiteViewModel model;

        [TestInitialize]
        public void Init()
        {
            model = SiteViewModelObjectBuilder
                .NewObject()
                .ThatPassesCheckAnswersValidation()
                .Build();
        }

        [TestMethod] 
        public void AllDataIsProvided()
        {
            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void FailWhenBasicDataIsNotProvided()
        {
            model.Name = "";
            model.ManyHomes = "";
            model.TypeHomes = new string[0];
            model.Type = "";
            model.AffordableHomes = "";
            model.ChargesDebt = "";
            model.HomesToBuild = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailReferenceNumberIsNotProvided()
        {
            model.PlanningRef = null;

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsNoAndNoLocationWasProvided()
        {
            model.PlanningRef = "No";
            model.LocationOption = "coordinates";
            model.LocationCoordinates = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsNoAndLandRegistryIsNotProvided()
        {
            model.PlanningRef = "No";
            model.LocationOption = "landRegistryTitleNumber";
            model.LocationLandRegistry = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndReferenceNumberIsNotProvided()
        {
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndPlanningStatusIsNotProvided()
        {
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndLocationIsNotProvided()
        {
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.LocationOption = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndCoordinatesAreNotProvided()
        {   
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.LocationOption = "coordinates";
            model.LocationCoordinates = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndLandRegistryIsNotProvided()
        {
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.LocationOption = "landRegistryTitleNumber";
            model.LocationCoordinates = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void PassWhenAllPlanningRefDataIsProvided()
        {
            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void FailWhenOwnershipIsNotProvided()
        {
            model.Ownership = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenOwnershipIsYesAndAdditionalDataIsNotProvided()
        {
            model.Ownership = "Yes";

            model.PurchaseDate = null;
            model.Cost = "";
            model.Value = "";
            model.Type = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void PassWhenOwnershipIsYesAndAdditionalDataIsProvided()
        {
            model.Ownership = "Yes";

            model.PurchaseDate = new DateTime(2023, 7, 7);
            model.Cost = "123";
            model.Value = "123";
            model.Type = "type";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void PassWhenOwnershipIsNoAndAdditionalDataIsNotProvided()
        {
            model.Ownership = "No";

            model.PurchaseDate = null;
            model.Cost = "";
            model.Value = "";
            model.Source = "";

            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void FailWhenGrantFundingIsNotProvided()
        {
            model.GrantFunding = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenGrantFundingIsYesAndNoAdditionalInformationIsProvided()
        {
            model.GrantFunding = "Yes";

            model.GrantFundingName = "";
            model.GrantFundingSource = "";
            model.GrantFundingAmount = "";
            model.GrantFundingPurpose = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }


        [TestMethod]
        public void PassWhenGrantFundingIsYesAndAllAdditionalInformationIsProvided()
        {
            model.GrantFunding = "Yes";

            model.GrantFundingName = "name";
            model.GrantFundingSource = "source";
            model.GrantFundingAmount = "12";
            model.GrantFundingPurpose = "purpose";

            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void PassWhenGrantFundingIsNoAndNoAdditionalInformationIsProvided()
        {
            model.GrantFunding = "No";

            model.GrantFundingName = "";
            model.GrantFundingSource = "";
            model.GrantFundingAmount = "";
            model.GrantFundingPurpose = "";

            model.AllInformationIsProvided().Should().BeTrue();
        }

    }
}
