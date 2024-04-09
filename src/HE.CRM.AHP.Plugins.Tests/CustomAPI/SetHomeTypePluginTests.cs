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
using System.Text.Json;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.AHP.Plugins.Tests.CustomApi
{
    [TestClass]
    public class SetHomeTypePluginTests
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
        public void Test_Update()
        {
            var homeTypeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            var hometype = new HomeTypeDto()
            {
                id = homeTypeId.ToString(),
                homeTypeName = "abc"
            };

            var contact = new Contact()
            {
                Id = userId,
                invln_externalid = userId.ToString()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                new invln_HomeType()
                {
                    Id = homeTypeId,
                    invln_application = applicationId.ToEntityReference<invln_scheme>()
                },
                contact,
                new Account()
                {
                    Id = organizationId
                },
                new invln_scheme()
                {
                    Id = applicationId,
                    invln_organisationid = organizationId.ToEntityReference<Account>()
                }
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
                { invln_sethometypeRequest.Fields.invln_hometype, JsonSerializer.Serialize(hometype)},
                { invln_sethometypeRequest.Fields.invln_userid, userId.ToString()},
                { invln_sethometypeRequest.Fields.invln_organisationid, organizationId.ToString()},
                { invln_sethometypeRequest.Fields.invln_applicationid, applicationId.ToString()},
                { invln_sethometypeRequest.Fields.invln_fieldstoset, null}
            };

            fakedContext.ExecutePluginWithConfigurations<SetHomeTypePlugin>(pluginContext, "", "");

            var homeTypesList = fakedContext.CreateQuery<invln_HomeType>().Where(x => x.invln_hometypename != null).ToList();
            Assert.AreEqual(1, homeTypesList.Count);
        }

        [TestMethod]
        public void Test_Create()
        {
            var homeTypeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            var hometype = new HomeTypeDto()
            {
                homeTypeName = "abc"
            };

            var contact = new Contact()
            {
                Id = userId,
                invln_externalid = userId.ToString()
            };

            fakedContext.Initialize(new List<Entity>()
            {
                contact,
                new Account()
                {
                    Id = organizationId
                },
                new invln_scheme()
                {
                    Id = applicationId,
                    invln_organisationid = organizationId.ToEntityReference<Account>()
                }
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
                { invln_sethometypeRequest.Fields.invln_hometype, JsonSerializer.Serialize(hometype)},
                { invln_sethometypeRequest.Fields.invln_userid, userId.ToString()},
                { invln_sethometypeRequest.Fields.invln_organisationid, organizationId.ToString()},
                { invln_sethometypeRequest.Fields.invln_applicationid, applicationId.ToString()},
                { invln_sethometypeRequest.Fields.invln_fieldstoset, null}
            };

            fakedContext.ExecutePluginWithConfigurations<SetHomeTypePlugin>(pluginContext, "", "");

            var homeTypesList = fakedContext.CreateQuery<invln_HomeType>().Where(x => x.invln_hometypename != null).ToList();
            Assert.AreEqual(1, homeTypesList.Count);
        }
    }
}
