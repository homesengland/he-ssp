using System;
using System.Collections.Generic;
using System.Text.Json;
using DataverseModel;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using HE.Base.Common.Extensions;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;

namespace HE.CRM.AHP.Plugins.Tests.CustomAPI
{
    [TestClass]
    public class SetDeliveryPhaseTests
    {
        private XrmFakedContext fakedContext;

        private XrmFakedPluginExecutionContext pluginContext;

        [TestInitialize]
        public void Initialize()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext = new XrmFakedContext();
#pragma warning restore CS0618 // Type or member is obsolete
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
        public void SetDeliveryPhase_Test()
        {
            var applicatioId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var programmeId = Guid.NewGuid();
            var milestone_1 = Guid.NewGuid();
            var milestone_2 = Guid.NewGuid();
            var milestone_3 = Guid.NewGuid();
            var deliveryPhaseId_1 = Guid.NewGuid();
            var deliveryPhaseId_2 = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            var contact = new Contact()
            {
                Id = contactId,
                invln_externalid = contactId.ToString()
            };

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(new List<Entity>()
            {
                new Account()
                {
                    Id = organizationId,
                    invln_UnregisteredBody = false,
                },
                new invln_scheme()
                {
                Id = applicatioId,
                invln_programmelookup = programmeId.ToEntityReference<invln_programme>(),
                invln_contactid = contactId.ToEntityReference<Contact>(),
                invln_noofhomes = 10,
                invln_fundingrequired = new Money(10000),
                invln_organisationid = organizationId.ToEntityReference<Account>(),
                },
                contact,
                new invln_programme()
                {
                    Id = programmeId
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_1,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.Acquisition),
                    invln_percentagepaidonmilestone = 40
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_2,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.SoS),
                    invln_percentagepaidonmilestone = 35
                },
                new invln_milestoneframeworkitem()
                {
                    Id= milestone_3,
                    invln_programmeId = programmeId.ToEntityReference<invln_programme>(),
                    invln_milestone = new OptionSetValue((int)invln_Milestone.PC),
                    invln_percentagepaidonmilestone = 25
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_1,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 5,
                    invln_AcquisitionPercentageValue = 40m,
                    invln_StartOnSitePercentageValue = 35m,
                    invln_CompletionPercentageValue = 25m,
                    invln_AcquisitionValue = new Money(4000),
                    invln_StartOnSiteValue = new Money(4000),
                    invln_CompletionValue = new Money(3000)
                },
                new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseId_2,
                    invln_Application = applicatioId.ToEntityReference<invln_scheme>(),
                    invln_NoofHomes = 5
                },
            })
            ;
#pragma warning restore CS0618 // Type or member is obsolete

            var metadata = fakedContext.GetEntityMetadataByName("contact");
            var keymetadata = new EntityKeyMetadata[]
            {
                new EntityKeyMetadata()
                {
                    KeyAttributes = new string[]{ "invln_externalid" }
                }
            };
            metadata.SetFieldValue("_keys", keymetadata);
            fakedContext.SetEntityMetadata(metadata);
            contact.KeyAttributes.Add("invln_externalid", contactId.ToString());
            DeliveryPhaseDto dFDto = new DeliveryPhaseDto()
            {
                id = deliveryPhaseId_1.ToString(),
                acquisitionPercentageValue = 0.3m,
                numberOfHomes = new Dictionary<string, int?>
                {
                    { deliveryPhaseId_1.ToString(), 3},
                    { deliveryPhaseId_2.ToString(), 2}
                }
            };

            pluginContext.InputParameters = new ParameterCollection
            {
                {invln_setdeliveryphaseRequest.Fields.invln_deliveryPhase, JsonSerializer.Serialize(dFDto) },
                {invln_setdeliveryphaseRequest.Fields.invln_userId, contactId.ToString() },
                {invln_setdeliveryphaseRequest.Fields.invln_organisationId, organizationId.ToString()},
                {invln_setdeliveryphaseRequest.Fields.invln_applicationId, applicatioId.ToString()},
                {invln_setdeliveryphaseRequest.Fields.invln_fieldstoset,""}
            };
            fakedContext.ExecutePluginWithConfigurations<SetDeliveryPhasePlugin>(pluginContext, "", "");
        }
    }
}
