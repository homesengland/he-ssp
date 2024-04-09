using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class SetApplicationPluginTests
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
        public void SetApplicationPluginTests_1()
        {
            var applicationId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var app = new AhpApplicationDto()
            {
                id = applicationId.ToString(),
                expectedOnCosts = 101
            };
            var contact = new Contact()
            {
                Id = userId,
                invln_externalid = userId.ToString()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_scheme()
                {
                    Id = applicationId,
                },
                new Account()
                {
                    Id = organizationId
                },
                contact
            });

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
            contact.KeyAttributes.Add("invln_externalid", userId.ToString());

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_setahpapplicationRequest.Fields.invln_application, JsonSerializer.Serialize(app)},
                { invln_setahpapplicationRequest.Fields.invln_fieldstoupdate, $"{invln_scheme.Fields.invln_oncosts}"},
                { invln_setahpapplicationRequest.Fields.invln_organisationid, organizationId.ToString()},
                { invln_setahpapplicationRequest.Fields.invln_userid, userId.ToString()}
            };

            fakedContext.ExecutePluginWithConfigurations<SetApplicationPlugin>(pluginContext, "", "");
            var output = pluginContext.OutputParameters[invln_setahpapplicationResponse.Fields.invln_applicationid].ToString();
            Assert.AreEqual(applicationId.ToString(), output);
        }
    }
}
