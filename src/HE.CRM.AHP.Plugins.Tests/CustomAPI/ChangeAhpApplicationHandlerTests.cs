using DataverseModel;
using FakeItEasy;
using FakeXrmEasy;
using HE.CRM.AHP.Plugins.Plugins.CustomApi;
using HE.Base.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Metadata;
using FakeXrmEasy.Extensions;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class ChangeAhpApplicationHandlerTests
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
        public void ChangeAhpApplicationHandler_CustomApi_Tests()
        {
            var schemaID = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            var contact = new Contact()
            {
                Id = contactId,
                invln_externalid = contactId.ToString()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_scheme()
                {
                    Id = schemaID,
                    invln_organisationid = accountId.ToEntityReference<Account>(),
                    invln_contactid = contactId.ToEntityReference<Contact>(),
                    invln_ExternalStatus = new OptionSetValue((int)invln_ExternalStatusAHP.Draft),
                    StatusCode = new OptionSetValue((int)invln_scheme_StatusCode.Draft)
                },
                new Account() { Id= accountId },
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
            contact.KeyAttributes.Add("invln_externalid", contactId.ToString());

            try
            {
                var request = new invln_changeahpapplicationstatusRequest();
                pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_changeahpapplicationstatusRequest.Fields.invln_applicationid,
                                                                    schemaID.ToString()
                    },
                    {invln_changeahpapplicationstatusRequest.Fields.invln_userid,
                                                                    contactId.ToString()
                    },
                    {invln_changeahpapplicationstatusRequest.Fields.invln_organisationid,
                                                                    accountId.ToString()
                    },
                    {invln_changeahpapplicationstatusRequest.Fields.invln_newapplicationstatus,
                                                                    (int)invln_ExternalStatusAHP.Draft
                    },
                };

                fakedContext.ExecutePluginWithConfigurations<ChangeAhpApplicationStatusPlugin>(pluginContext, "", "");
            }
            catch (Exception ex)
            {
            }

            var schema = fakedContext.CreateQuery<invln_scheme>().FirstOrDefault();
            Assert.IsNotNull(schema);
            Assert.AreEqual(expected: (int)invln_ExternalStatusAHP.Draft, schema.invln_ExternalStatus.Value);
        }
    }
}
