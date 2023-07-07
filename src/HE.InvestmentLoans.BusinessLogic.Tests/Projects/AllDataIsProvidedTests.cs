using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects
{
    [TestClass]
    public class AllDataIsProvidedTests
    {
        [TestMethod] 
        public void AllDataIsProvided()
        {
            var model = ModelThatPassesValidation();

            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void FailWhenBasicDataIsNotProvided()
        {
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

            model.PlanningRef = null;

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsNoAndNoLocationWasProvided()
        {
            var model = ModelThatPassesValidation();

            model.PlanningRef = "No";
            model.LocationOption = "coordinates";
            model.LocationCoordinates = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsNoAndLandRegistryIsNotProvided()
        {
            var model = ModelThatPassesValidation();

            model.PlanningRef = "No";
            model.LocationOption = "landRegistryTitleNumber";
            model.LocationLandRegistry = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndReferenceNumberIsNotProvided()
        {
            var model = ModelThatPassesValidation();

            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndPlanningStatusIsNotProvided()
        {
            var model = ModelThatPassesValidation();

            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndLocationIsNotProvided()
        {
            var model = ModelThatPassesValidation();

            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.LocationOption = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenPlanningRefIsYesAndCoordinatesAreNotProvided()
        {
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

            model.PlanningRef = "Yes";
            model.PlanningRefEnter = "number";
            model.PlanningStatus = "status";

            model.AllInformationIsProvided().Should().BeTrue();
        }

        [TestMethod]
        public void FailWhenOwnershipIsNotProvided()
        {
            var model = ModelThatPassesValidation();

            model.Ownership = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenOwnershipIsYesAndAdditionalDataIsNotProvided()
        {
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

            model.GrantFunding = "";

            model.AllInformationIsProvided().Should().BeFalse();
        }

        [TestMethod]
        public void FailWhenGrantFundingIsYesAndNoAdditionalInformationIsProvided()
        {
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

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
            var model = ModelThatPassesValidation();

            model.GrantFunding = "No";

            model.GrantFundingName = "";
            model.GrantFundingSource = "";
            model.GrantFundingAmount = "";
            model.GrantFundingPurpose = "";

            model.AllInformationIsProvided().Should().BeTrue();
        }

        private static SiteViewModel ModelThatPassesValidation()
        {
            return new SiteViewModel
            {
                Name = "Test",
                ManyHomes = "12",
                HasEstimatedStartDate = "No",
                TypeHomes = new string[] { "tmp" },
                Type = "greenfield",
                AffordableHomes = "No",
                ChargesDebt = "No",
                HomesToBuild = "12",

                GrantFunding = "No",
                PlanningRef = "No",
                LocationOption = "coordinates",
                LocationCoordinates = "12,12 12,12",
                Ownership = "No"
            };
        }
    }
}
