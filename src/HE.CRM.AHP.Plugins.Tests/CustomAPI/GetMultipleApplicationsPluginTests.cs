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
    public class GetMultipleApplicationsPluginTests
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
        public void Test_1()
        {
            var contactId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var appId_1 = Guid.NewGuid();
            var appId_2 = Guid.NewGuid();

            var contact = new Contact()
            {
                Id = contactId,
                invln_externalid = contactId.ToString()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new Account()
                {
                    Id= accountId
                },
                new invln_scheme()
                { Id = appId_1,
                    invln_organisationid = accountId.ToEntityReference<Account>(),
                    invln_contactid = contactId.ToEntityReference<Contact>()
                },
                new invln_scheme()
                { Id = appId_2,
                    invln_organisationid = accountId.ToEntityReference<Account>(),
                    invln_contactid = contactId.ToEntityReference<Contact>()
                },
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

            pluginContext.InputParameters = new ParameterCollection
            {
                { invln_getmultipleahpapplicationsRequest.Fields.invln_appfieldstoretrieve, null},
                { invln_getmultipleahpapplicationsRequest.Fields.inlvn_userid, contactId.ToString()},
                { invln_getmultipleahpapplicationsRequest.Fields.invln_organisationid, accountId.ToString()}
            };

            fakedContext.ExecutePluginWithConfigurations<GetMultipleApplicationsPlugin>(pluginContext, "", "");
            var outputResponce = pluginContext.OutputParameters[invln_getmultipleahpapplicationsResponse.Fields.invln_ahpapplications].ToString();
            Assert.IsNotNull(outputResponce);
            Assert.AreNotEqual("[]", outputResponce);
        }
    }
}
