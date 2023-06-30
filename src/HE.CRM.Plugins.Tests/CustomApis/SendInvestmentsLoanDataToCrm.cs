using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class SendInvestmentsLoanDataToCrm
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;
        private LoanApplicationDto applicationDto;
        private SiteDetailsDto siteDetailsDto;
        string payload = String.Empty;

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
            Exception exception = null;
            try
            {
                var request = new invln_sendinvestmentloansdatatocrmRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_entityfieldsparameters), String.Empty }
                };

                fakedContext.ExecutePluginWithConfigurations<SendInvestmentsLoanDataToCrmPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
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
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<Contact>.Ignored)).MustHaveHappened();
        }

        [TestMethod]
        public void SendInvestmentsLoanDataToCrm_ContactAlreadyExistsInCrm_ShouldDeleteExistingInvestmentLoanAndCreateNew()
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                EMailAddress1 = applicationDto.contactEmailAdress,
            };

            invln_Loanapplication existingLoan = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
                invln_Contact = contact.ToEntityReference(),
            };

            fakedContext.Initialize(new List<Entity> { existingLoan, contact });

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
            A.CallTo(() => fakedContext.GetOrganizationService().Delete(existingLoan.LogicalName, existingLoan.Id)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustHaveHappened();

        }

        [TestMethod]
        public void SendInvesmentsLoanDataToCrm_PayloadDataWithObjects_ShouldCreateMultipleSiteDetails()
        {
            SiteDetailsDto site2 = siteDetailsDto;
            SiteDetailsDto site3 = siteDetailsDto;
            SiteDetailsDto site4 = siteDetailsDto;
            LoanApplicationDto loan = applicationDto;
            loan.siteDetailsList.Add(site2);
            loan.siteDetailsList.Add(site3);
            loan.siteDetailsList.Add(site4);
            string newPayload = JsonSerializer.Serialize<LoanApplicationDto>(loan);
            Exception exception = null;

            try
            {
                var request = new invln_sendinvestmentloansdatatocrmRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_entityfieldsparameters), newPayload }
                };

                fakedContext.ExecutePluginWithConfigurations<SendInvestmentsLoanDataToCrmPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustHaveHappenedTwiceOrMore();
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
                typeOfHomes = new string[] { "typeOfHomes" },
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

                siteDetailsList = new List<SiteDetailsDto> { siteDetailsDto },

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

            payload = JsonSerializer.Serialize<LoanApplicationDto>(applicationDto);
        }
    }
}
