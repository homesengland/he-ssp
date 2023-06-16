using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class SendInvestmentsLoanDataToCrm
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;
        private LoanApplicationDto applicationDto;
        private SiteDetailsDto siteDetailsDto;
        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
            InitData();
        }

        [TestMethod]
        public void SendInvesmentsLoanDataToCrm_PayloadDataNull_PayloadDataIsNull_ShouldCreateNothing()
        {
            string payload = "";
            Exception exception = null;
            try
            {
                var request = new invln_sendinvestmentloansdatatocrmRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_entityfieldsparameters), payload }
                };

                fakedContext.ExecutePluginWithConfigurations<SendInvestmentsLoanDataToCrmPlugin>(pluginContext, "", "");
            }catch(Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_Loanapplication>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<Contact>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        public void SendInvesmentsLoanDataToCrm_PayloadDataWithObjects_ShouldCreateNewRecords()
        {
            string payload = JsonSerializer.Serialize<LoanApplicationDto>(applicationDto);
            Exception exception = null;
            try
            {
                var request = new invln_sendinvestmentloansdatatocrmRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_entityfieldsparameters), payload }
                };

                fakedContext.ExecutePluginWithConfigurations<SendInvestmentsLoanDataToCrmPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            //A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustHaveHappened(); // commented in code currently
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<Contact>.Ignored)).MustHaveHappened();
        }

        private void InitData()
        {
            siteDetailsDto = new SiteDetailsDto()
            {
                currentValue = "value",
                dateOfPurchase = DateTime.Now,
                existingLegalCharges = "existingLegalCharges",
                existingLegalChargesInformation = "existingLegalChargesInformation",
                haveAPlanningReferenceNumber = "haveAPlanningReferenceNumber",
                howMuch = "howMuch",
                landRegistryTitleNumber = "landRegistryTitleNumber",
                Name = "Name",
                nameOfGrantFund = "nameOfGrantFund",
                numberOfAffordableHomes = "numberOfAffordableHomes",
                numberOfHomes = "numberOfHomes",
                otherTypeOfHomes = "otherTypeOfHomes",
                planningReferenceNumber = "planningReferenceNumber",
                publicSectorFunding = "publicSectorFunding",
                reason = "reason",
                siteCoordinates = "siteCoordinates",
                siteCost = "siteCost",
                siteName = "siteName",
                siteOwnership = "siteOwnership",
                typeOfHomes = new string[]{ "typeOfHomes" },
                typeOfSite = "typeOfSite",
                valuationSource = "valuationSource",
                whoProvided = "whoProvided",
            };

            applicationDto = new LoanApplicationDto()
            {
                companyPurpose = "true",
                existingCompany = "",
                companyExperience = 5,

                projectGdv = "22.2",
                projectEstimatedTotalCost = "33.3",
                projectAbnormalCosts = "true",
                projectAbnormalCostsInformation = "projectAbnormalCostsInformation",
                privateSectorApproach = "false",
                privateSectorApproachInformation = "privateSectorApproachInformation",
                additionalProjects = "true",
                //refinanceRepayment = "",
                refinanceRepaymentDetails = "refinanceRepaymentDetails",

                outstandingLegalChargesOrDebt = "false",
                debentureHolder = "debentureHolder",
                directorLoans = "true",
                confirmationDirectorLoansCanBeSubordinated = "false",
                reasonForDirectorLoanNotSubordinated = "true",

                siteDetailsList = new List<SiteDetailsDto> { siteDetailsDto},

                //id = "",
                name = "name",
                numberOfSites = "20",
                companyStructureInformation = "companyStructureInformation",
                costsForAdditionalProjects = "",
                fundingReason = "Buildinginfrastructureonly",
                fundingTypeForAdditionalProjects = "fundingTypeForAdditionalProjects",
                contactEmailAdress = "test@test.pl",
                //accountId = "",
            };
        }
    }
}
