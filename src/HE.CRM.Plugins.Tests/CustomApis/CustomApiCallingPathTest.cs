using DataverseModel;
using FakeItEasy;
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

        private LoanApplicationDto applicationDto;
        private SiteDetailsDto siteDetailsDto;

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

            this.Init();
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

            var getContactRoleOutput = this.CallGetContactRolePlugin(contact.invln_externalid, portal.invln_Portal.Value.ToString(), contact.EMailAddress1, false);
            Assert.IsNotNull(getContactRoleOutput);
            Assert.IsNotNull(getContactRoleOutput.contactRoles);

            Assert.AreEqual(role.invln_Name, getContactRoleOutput.contactRoles.First().webRoleName);

            var getOrganizationDetailsOutput = this.CallGetOrganizationDetailsPlugin(account.Id.ToString(), contact.invln_externalid, false);

            Assert.IsNotNull(getOrganizationDetailsOutput);
            Assert.AreEqual(account.Name, getOrganizationDetailsOutput.registeredCompanyName);

            var sendInvestmentLoanDataOutput = this.CallSendInvestmentsLoanDataToCrm(getContactRoleOutput.externalId, account.Id.ToString(), String.Empty, JsonSerializer.Serialize(applicationDto), false);

            Assert.IsNotNull(sendInvestmentLoanDataOutput);
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_Loanapplication>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakedContext.GetOrganizationService().Create(A<invln_SiteDetails>.Ignored)).MustHaveHappened();

            var getLoanApplicationOutput1 = this.CallGetSingleLoanApplicationPlugin(account.Id.ToString(), getContactRoleOutput.externalId, sendInvestmentLoanDataOutput, false);

            Assert.IsNotNull(getLoanApplicationOutput1);
            Assert.AreEqual(getLoanApplicationOutput1.name, applicationDto.name);

            var loanDtoNewFieldsValues = new LoanApplicationDto()
            {
                projectAbnormalCostsInformation = "projectAbnormalCostsInformationchangetest",
            };

            this.CallUpdateSingleLoanApplication(JsonSerializer.Serialize(loanDtoNewFieldsValues), "invln_projectabnormalcostsinformation", getLoanApplicationOutput1.loanApplicationId, account.Id.ToString(), getContactRoleOutput.externalId, false);
            A.CallTo(() => fakedContext.GetOrganizationService().Update(A<invln_Loanapplication>.That.Matches(x => x.Id.ToString() == getLoanApplicationOutput1.loanApplicationId))).MustHaveHappened();

            this.CallUpdateExternalStatus(getLoanApplicationOutput1.loanApplicationId, (int)invln_ExternalStatus.CPssatisfied, false);

            var getLoanApplicationOutput2 = this.CallGetSingleLoanApplicationPlugin(account.Id.ToString(), getContactRoleOutput.externalId, sendInvestmentLoanDataOutput, false);
            Assert.AreEqual(getLoanApplicationOutput1.loanApplicationId, getLoanApplicationOutput2.loanApplicationId);
            Assert.AreNotEqual(getLoanApplicationOutput1.loanApplicationExternalStatus, getLoanApplicationOutput2.loanApplicationExternalStatus);
        }

        private void CallUpdateExternalStatus(string loanApplicationId, int externalStatus, bool shouldThrowError)
        {
            Exception exception = null;
            try
            {
                var request = new invln_changeloanapplicationexternalstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplicationid), loanApplicationId },
                    {nameof(request.invln_statusexternal), externalStatus }
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeLoanApplicationExternalStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
            }
            else
            {
                Assert.IsNull(exception);
            }
        }

        private void CallUpdateSingleLoanApplication(string loanApplication, string fieldsToUpdate, string loanApplicationId, string accountId, string contactExternalId, bool shouldThrowError)
        {
            Exception exception = null;
            try
            {
                var request = new invln_updatesingleloanapplicationRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {nameof(request.invln_loanapplication), loanApplication },
                    {nameof(request.invln_fieldstoupdate), fieldsToUpdate },
                    {nameof(request.invln_loanapplicationid), loanApplicationId },
                    {nameof(request.invln_accountid), accountId },
                    {nameof(request.invln_contactexternalid), contactExternalId },
                };

                fakedContext.ExecutePluginWithConfigurations<UpdateSingleLoanApplicationPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
            }
            else
            {
                Assert.IsNull(exception);
            }
        }

        private List<LoanApplicationDto> CallGetLoanApplicationForAccountAndcontact(string accountId, string externalContactId, bool shouldThrowError)
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
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
                return null;
            }
            else
            {
                var deserializedLoanApplicationDtoList = JsonSerializer.Deserialize<List<LoanApplicationDto>>(pluginContext.OutputParameters["invln_loanapplications"].ToString());

                Assert.IsNull(exception);
                return deserializedLoanApplicationDtoList;
            }
        }

        private LoanApplicationDto CallGetSingleLoanApplicationPlugin(string accountId, string externalContactId, string loanApplicationId, bool shouldThrowError)
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
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
                return null;
            }
            else
            {
                var deserializedLoanApplicationDtoList = JsonSerializer.Deserialize<List<LoanApplicationDto>>(pluginContext.OutputParameters["invln_loanapplication"].ToString());

                Assert.IsNull(exception);
                return deserializedLoanApplicationDtoList?.First();
            }
        }

        private OrganizationDetailsDto CallGetOrganizationDetailsPlugin(string accountId, string externalContactId, bool shouldThrowError)
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
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
                return null;
            }
            else
            {
                var deserializedOrganizationDetails = JsonSerializer.Deserialize<OrganizationDetailsDto>(pluginContext.OutputParameters["invln_organizationdetails"].ToString());

                Assert.IsNull(exception);
                return deserializedOrganizationDetails;
            }
        }

        private ContactRolesDto CallGetContactRolePlugin(string contactExternalId, string portalType, string emailAddress, bool shouldThrowErrors)
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

            if (shouldThrowErrors)
            {
                Assert.IsNotNull(exception);
                return null;
            }
            else
            {
                var deserializedContactRoles = JsonSerializer.Deserialize<ContactRolesDto>(pluginContext.OutputParameters["invln_portalroles"].ToString());

                Assert.IsNull(exception);
                return deserializedContactRoles;
            }
        }

        private string CallSendInvestmentsLoanDataToCrm(string contactExternalId, string accountId, string loanApplicationId, string requestStringMessage, bool shouldThrowError)
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
            if (shouldThrowError)
            {
                Assert.IsNotNull(exception);
                return String.Empty;
            }
            else
            {
                var loanAppId = pluginContext.OutputParameters["invln_loanapplicationid"].ToString();
                Assert.IsNull(exception);
                return loanAppId;
            }
        }

        private void Init()
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
                refinanceRepayment = "",
                refinanceRepaymentDetails = "refinanceRepaymentDetails",

                outstandingLegalChargesOrDebt = "false",
                debentureHolder = "debentureHolder",
                directorLoans = "true",
                confirmationDirectorLoansCanBeSubordinated = "false",
                reasonForDirectorLoanNotSubordinated = "true",

                siteDetailsList = new List<SiteDetailsDto> { siteDetailsDto },

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
            };
        }
    }
}
