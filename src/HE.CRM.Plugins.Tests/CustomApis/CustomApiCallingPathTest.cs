using DataverseModel;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;

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
            var entityMetadata = new EntityMetadata()
            {
                LogicalName = "contact",
            };
            var nameAttribute = new StringAttributeMetadata()
            {
                LogicalName = "invln_externalid",
                RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.ApplicationRequired)
            };
            entityMetadata.SetAttributeCollection(new[] { nameAttribute });

            fakedContext.InitializeMetadata(entityMetadata);
            entityMetadata.LogicalName = "invln_loanapplication";
            fakedContext.InitializeMetadata(entityMetadata);
        }

        [TestMethod]
        public void RoleRetrieved_OrganizationDetailsRetrieved_SubmitCreatedNewRecord_SingleLoanApplicationRetrieved_LoanApplicationUpdated_LoanStatusChanged_LoanWithNewDataRetrieved()
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),
                EMailAddress1 = "test@test.pl",
                invln_externalid = "2137",
            };

            invln_portal portal = new invln_portal()
            {
                Id = Guid.NewGuid(),
                invln_Portal = new OptionSetValue((int)invln_Portal1.Loans),
            };

            invln_Webrole role = new invln_Webrole()
            {
                Id = Guid.NewGuid(),
                invln_Portalid = portal.ToEntityReference(),
                invln_Name = "role name",
            };

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                Name = "account name",
            };

            var relationship = new invln_contactwebrole()
            {
                Id = Guid.NewGuid(),
                invln_Contactid = contact.ToEntityReference(),
                invln_Webroleid = role.ToEntityReference(),
                invln_Accountid = account.ToEntityReference(),
            };

            fakedContext.Initialize(new List<Entity> { contact, role, relationship, portal, account });

            var metadata = fakedContext.GetEntityMetadataByName(Contact.EntityLogicalName);
            var keymetadata = new EntityKeyMetadata[]
            {
                new EntityKeyMetadata()
                {
                    KeyAttributes = new string[]{ nameof(Contact.invln_externalid) }
                }
            };
            metadata.SetFieldValue("_keys", keymetadata);
            fakedContext.SetEntityMetadata(metadata);
            contact.KeyAttributes.Add(nameof(Contact.invln_externalid), contact.invln_externalid);

            var GetContactRoleOutput = this.CallGetContactRolePlugin(contact.invln_externalid, portal.invln_Portal.Value.ToString(), contact.EMailAddress1);
            Assert.IsNotNull(GetContactRoleOutput);
            Assert.IsNotNull(GetContactRoleOutput.contactRoles);
            Assert.AreEqual(role.invln_Name, GetContactRoleOutput.contactRoles.First().webRoleName);

            var GetOrganizationDetailsOutput = this.CallGetOrganizationDetailsPlugin(account.Id.ToString(), contact.invln_externalid);

            Assert.IsNotNull(GetOrganizationDetailsOutput);
            Assert.AreEqual(account.Name, GetOrganizationDetailsOutput.registeredCompanyName);
        }

        private List<LoanApplicationDto> CallGetLoanApplicationForAccountAndcontact(string accountId, string externalContactId)
        {
            Exception exception = null;
            try
            {
                var request = new invln_getloanapplicationsforaccountandcontactRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_accountid), accountId },
                    {nameof(request.invln_externalcontactid), externalContactId },
                };

                fakedContext.ExecutePluginWithConfigurations<GetInvestmentsLoansForAccountAndContactPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var deserializedLoanApplicationDtoList = JsonSerializer.Deserialize<List<LoanApplicationDto>>(pluginContext.OutputParameters.Values.ElementAt(0).ToString());

            Assert.IsNull(exception);
            return deserializedLoanApplicationDtoList;
        }

        private LoanApplicationDto CallGetSingleLoanApplicationPlugin(string accountId, string externalContactId, string loanApplicationId)
        {
            Exception exception = null;
            try
            {
                var request = new invln_getsingleloanapplicationforaccountandcontactRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_accountid), accountId },
                    {nameof(request.invln_externalcontactid), externalContactId },
                    {nameof(request.invln_loanapplicationid), loanApplicationId },
                };

                fakedContext.ExecutePluginWithConfigurations<GetSingleInvestmentLoanForAccountAndContactPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var deserializedLoanApplicationDtoList = JsonSerializer.Deserialize<List<LoanApplicationDto>>(pluginContext.OutputParameters.Values.ElementAt(0).ToString());

            Assert.IsNull(exception);
            return deserializedLoanApplicationDtoList?.First();
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
