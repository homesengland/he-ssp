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
using System.Text.Json.Serialization;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class UpdateSingleLoanApplicationTest
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;
        private IOrganizationService fakedService;
        private LoanApplicationDto applicationDto;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
            fakedService = fakedContext.GetOrganizationService();
            Init();
        }

        [TestMethod]
        public void UpdateSingleLoanApplication_NoLoanApplicationWithGivenIdExists_ShouldThrowError()
        {
            Exception exception = null;
            try
            {
                var request = new invln_updatesingleloanapplicationRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_contactexternalid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_accountid), Guid.NewGuid().ToString() },
                    {nameof(request.invln_loanapplication), JsonSerializer.Serialize(applicationDto) },
                    {nameof(request.invln_fieldstoupdate), string.Empty },
                };

                fakedContext.ExecutePluginWithConfigurations<UpdateSingleLoanApplicationPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        //[TestMethod]
        //public void UpdateSingleLoanApplication_FieldsAreGiven_ShouldUpdateGivenFields()
        //{
        //    invln_Loanapplication loanApplication = new invln_Loanapplication()
        //    {
        //        Id = Guid.Parse(applicationDto.loanApplicationId),
        //    };

        //    Contact contact = new Contact()
        //    {
        //        Id = Guid.NewGuid(),
        //        invln_externalid = "2137",
        //    };

        //    Account account = new Account()
        //    {
        //        Id = Guid.NewGuid(),
        //    };

        //    fakedContext.Initialize(new List<Entity> { loanApplication, contact, account });
        //    string fieldsToUpdate = $"{nameof(invln_Loanapplication.invln_Contact).ToLower()},{nameof(invln_Loanapplication.invln_Account).ToLower()},{nameof(invln_Loanapplication.invln_CompanyPurpose).ToLower()}";
        //    Exception exception = null;

        //    try
        //    {
        //        var request = new invln_updatesingleloanapplicationRequest();
        //        pluginContext.InputParameters = new ParameterCollection
        //        {
        //            {nameof(request.invln_loanapplicationid), loanApplication.Id.ToString() },
        //            {nameof(request.invln_contactexternalid), "TEST" },
        //            {nameof(request.invln_accountid), account.Id.ToString() },
        //            {nameof(request.invln_loanapplication), JsonSerializer.Serialize(applicationDto) },
        //            {nameof(request.invln_fieldstoupdate), fieldsToUpdate },
        //        };

        //        fakedContext.ExecutePluginWithConfigurations<UpdateSingleLoanApplicationPlugin>(pluginContext, "", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //    }
        //    var retrievedLoan = fakedService.Retrieve(invln_Loanapplication.EntityLogicalName, loanApplication.Id, new ColumnSet(true)).ToEntity<invln_Loanapplication>();
          
        //    A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
        //    Assert.IsNull(exception);
        //    Assert.IsNotNull(retrievedLoan.invln_CompanyPurpose);
        //    Assert.IsNotNull(retrievedLoan.invln_Account);
        //    Assert.IsNotNull(retrievedLoan.invln_Contact);
        //    Assert.AreNotEqual(loanApplication.invln_CompanyPurpose, retrievedLoan.invln_CompanyPurpose);
        //}
        //[TestMethod]
        //public void UpdateSingleLoanApplication_LoanApplicationWithGivenIdExists_ShouldUpdateRecord()
        //{
        //    invln_Loanapplication loanApplication = new invln_Loanapplication()
        //    {
        //        Id = Guid.Parse(applicationDto.loanApplicationId),
        //    };
        //    fakedContext.Initialize(new List<Entity> { loanApplication });

        //    Exception exception = null;
        //    try
        //    {
        //        var request = new invln_updatesingleloanapplicationRequest();
        //        pluginContext.InputParameters = new ParameterCollection
        //        {
        //            {nameof(request.invln_loanapplicationid), loanApplication.Id.ToString() },
        //            {nameof(request.invln_contactexternalid), Guid.NewGuid().ToString() },
        //            {nameof(request.invln_accountid), Guid.NewGuid().ToString() },
        //            {nameof(request.invln_loanapplication), JsonSerializer.Serialize(applicationDto) },
        //            {nameof(request.invln_fieldstoupdate), string.Empty },
        //        };

        //        fakedContext.ExecutePluginWithConfigurations<UpdateSingleLoanApplicationPlugin>(pluginContext, "", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //    }
        //    var retrievedLoan = fakedService.Retrieve(invln_Loanapplication.EntityLogicalName, loanApplication.Id, new ColumnSet(true)).ToEntity<invln_Loanapplication>();
        //    Assert.IsNull(exception);
        //    Assert.IsNull(retrievedLoan.invln_Contact);
        //    Assert.IsNotNull(retrievedLoan.invln_CompanyPurpose);
        //    Assert.AreNotEqual(loanApplication.invln_CompanyPurpose, retrievedLoan.invln_CompanyPurpose);
        //    A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
        //}


        private void Init()
        {
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

                loanApplicationId = Guid.NewGuid().ToString(),
                name = "name",
                numberOfSites = "20",
                companyStructureInformation = "companyStructureInformation",
                costsForAdditionalProjects = "",
                fundingReason = "Buildinginfrastructureonly",
                fundingTypeForAdditionalProjects = "fundingTypeForAdditionalProjects",
                contactEmailAdress = "test@test.pl",
                LoanApplicationContact = new UserAccountDto()
                {
                    ContactExternalId = "2137"
                }
                //accountId = "",
            };
        }
    }
}
