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
using System.Net.Mail;
using System.Text.Json;
using System.Web.Services.Description;

namespace HE.CRM.Plugins.Tests.CustomApis
{
    [TestClass]
    public class CustomApiCallingPathTest
    {
        private XrmFakedContext fakedContext;
        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
            fakedContext = new XrmFakedContext();
            pluginContext = fakedContext.GetDefaultPluginContext();
        }

        private OrganizationDetailsDto CallGetOrganizationDetailsPlugin(string accountId, string externalContactId)
        {
            Exception exception = null;
            try
            {
                var request = new invln_getorganizationdetailsRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_contactexternalid), externalContactId },
                    {nameof(request.invln_accountid), accountId },
                };

                fakedContext.ExecutePluginWithConfigurations<GetOrganizationDetailsPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var deserializedOrganizationDetails = JsonSerializer.Deserialize<OrganizationDetailsDto>(pluginContext.OutputParameters.Values.ElementAt(0).ToString());

            Assert.IsNull(exception);
            return deserializedOrganizationDetails;
        }

        private ContactRolesDto CallGetContactRolePlugin(string contactExternalId, string portalType, string emailAddress)
        {
            Exception exception = null;
            try
            {
                var request = new invln_getcontactroleRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_contactexternalid), contactExternalId },
                    {nameof(request.invln_portaltype), portalType },
                    {nameof(request.invln_contactemail), emailAddress },
                };

                fakedContext.ExecutePluginWithConfigurations<GetContactRolePlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var deserializedContactRoles = JsonSerializer.Deserialize<ContactRolesDto>(pluginContext.OutputParameters.Values.ElementAt(0).ToString());

            Assert.IsNull(exception);
            return deserializedContactRoles;
            //Assert.AreEqual(deserializedContactRoles.contactRoles.ElementAt(0).webRoleName, role.invln_Name);
        }

        private string CallSendInvestmentsLoanDataToCrm(string contactExternalId, string accountId, string loanApplicationId, string requestStringMessage)
        {
            Exception exception = null;
            try
            {
                var request = new invln_sendinvestmentloansdatatocrmRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_contactexternalid), contactExternalId },
                    {nameof(request.invln_accountid), accountId },
                    {nameof(request.invln_loanapplicationid), loanApplicationId },
                    {nameof(request.invln_entityfieldsparameters), requestStringMessage },
                };

                fakedContext.ExecutePluginWithConfigurations<SendInvestmentsLoanDataToCrmPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var loanAppId = pluginContext.OutputParameters.Values.ElementAt(0).ToString();

            Assert.IsNull(exception);
            return loanAppId;
        }
    }
}
