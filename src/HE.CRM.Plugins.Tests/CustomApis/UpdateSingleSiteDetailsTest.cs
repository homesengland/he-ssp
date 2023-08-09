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
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class UpdateSingleSiteDetailsTest
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;
        private IOrganizationService fakedService;
        private SiteDetailsDto siteDto;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
            fakedService = fakedContext.GetOrganizationService();
            Init();
        }


        [TestMethod]
        public void UpdateSingleSiteDetails_NoSiteDetailsWithGivenIdExists_ShouldThrowError()
        {
            Exception exception = null;
            try
            {
                var request = new invln_updatesinglesitedetailsRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_sitedetailsid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_sitedetail), JsonSerializer.Serialize(siteDto) },
                };

                fakedContext.ExecutePluginWithConfigurations<UpdateSingleSiteDetailsPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void UpdateSingleSiteDetails_FieldsAreGiven_ShouldUpdateGivenFields()
        {
            var loanApplication = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
            };

            var siteDetails = new invln_SiteDetails()
            {
                Id = Guid.NewGuid()
            };

            fakedContext.Initialize(new List<Entity> { loanApplication, siteDetails });
            var fieldsToUpdate = $"{nameof(invln_SiteDetails.invln_Existinglegalchargesinformation).ToLower()},{nameof(invln_SiteDetails.invln_Loanapplication).ToLower()},{nameof(invln_SiteDetails.invln_Planningreferencenumber).ToLower()}";
            Exception exception = null;
            try
            {
                var request = new invln_updatesinglesitedetailsRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), loanApplication.Id.ToString() },
                    {nameof(request.invln_sitedetailsid), siteDetails.Id.ToString() },
                    {nameof(request.invln_sitedetail), JsonSerializer.Serialize(siteDto) },
                    {nameof(request.invln_fieldstoupdate), fieldsToUpdate },
                };

                fakedContext.ExecutePluginWithConfigurations<UpdateSingleSiteDetailsPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var retrievedSiteDetail = fakedService.Retrieve(invln_SiteDetails.EntityLogicalName, siteDetails.Id, new ColumnSet(true)).ToEntity<invln_SiteDetails>();

            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_SiteDetails>.Ignored)).MustHaveHappened();
            Assert.IsNull(exception);
            Assert.IsNotNull(retrievedSiteDetail.invln_Existinglegalchargesinformation);
            Assert.IsNotNull(retrievedSiteDetail.invln_Loanapplication);
            Assert.IsNotNull(retrievedSiteDetail.invln_Planningreferencenumber);
            Assert.AreEqual(loanApplication.Id, retrievedSiteDetail.invln_Loanapplication.Id);
        }
        
        [TestMethod]
        public void UpdateSingleSiteDetails_LoanApplicationWithGivenIdExists_ShouldUpdateRecord()
        {
            var siteDetails = new invln_SiteDetails()
            {
                Id = Guid.NewGuid()
            };
            var loanApplication = new invln_Loanapplication()
            {
                Id = Guid.NewGuid(),
            };

            fakedContext.Initialize(new List<Entity> { siteDetails, loanApplication });

            Exception exception = null;
            try
            {
                var request = new invln_updatesinglesitedetailsRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), loanApplication.Id.ToString() },
                    {nameof(request.invln_sitedetailsid), siteDetails.Id.ToString() },
                    {nameof(request.invln_sitedetail), JsonSerializer.Serialize(siteDto) },
                };

                fakedContext.ExecutePluginWithConfigurations<UpdateSingleSiteDetailsPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var retrievedLoan = fakedService.Retrieve(invln_Loanapplication.EntityLogicalName, loanApplication.Id, new ColumnSet(true)).ToEntity<invln_Loanapplication>();
            var retrievedSiteDetail = fakedService.Retrieve(invln_SiteDetails.EntityLogicalName, siteDetails.Id, new ColumnSet(true)).ToEntity<invln_SiteDetails>();

            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_SiteDetails>.Ignored)).MustHaveHappened();
            Assert.IsNull(exception);
            Assert.IsNotNull(retrievedSiteDetail.invln_Existinglegalchargesinformation);
            Assert.IsNotNull(retrievedSiteDetail.invln_Loanapplication);
            Assert.IsNotNull(retrievedSiteDetail.invln_Planningreferencenumber);
            Assert.IsNotNull(retrievedSiteDetail.invln_Reason);
            Assert.IsNotNull(retrievedSiteDetail.invln_Nameofgrantfund);
            Assert.IsNotNull(retrievedSiteDetail.invln_Existinglegalchargesinformation);
            Assert.IsNotNull(retrievedSiteDetail.invln_Dateofpurchase);
            Assert.AreEqual(loanApplication.Id, retrievedSiteDetail.invln_Loanapplication.Id);
        }

        
        private void Init()
        {
            siteDto = new SiteDetailsDto()
            {
                currentValue = "value",
                dateOfPurchase = DateTime.Now,
                existingLegalCharges = true,
                existingLegalChargesInformation = "existingLegalChargesInformation",
                haveAPlanningReferenceNumber = true,
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
                siteOwnership = true,
                typeOfHomes = new string[] { "typeOfHomes" },
                typeOfSite = "typeOfSite",
                valuationSource = "valuationSource",
                whoProvided = "whoProvided",
            };
        }
    }
}
