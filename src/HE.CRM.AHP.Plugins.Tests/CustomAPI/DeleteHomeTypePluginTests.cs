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
    public class DeleteHomeTypePluginTests
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
        public void DeleteHomeTypePluginTest_1()
        {
            var homeTypeId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

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
                    invln_organisationid = organizationId.ToEntityReference<Account>()
                },
                new invln_HomeType()
                {
                    Id = homeTypeId,
                    invln_application = applicationId.ToEntityReference<invln_scheme>()
                },
                {
                    contact
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

            var request = new invln_deletehometypeRequest();
            pluginContext.InputParameters = new ParameterCollection
                {
                    {invln_deletehometypeRequest.Fields.invln_hometypeid, homeTypeId.ToString()},
                    {invln_deletehometypeRequest.Fields.invln_userid, userId.ToString()},
                    {invln_deletehometypeRequest.Fields.invln_organisationid, organizationId.ToString()},
                    {invln_deletehometypeRequest.Fields.invln_applicationid, applicationId.ToString()}
                };
            var homeTypesCountAfterDelete = fakedContext.CreateQuery<invln_HomeType>()
                                                        .Where(x => x.Id == homeTypeId)
                                                        .ToList().Count();
            Assert.AreEqual(1, homeTypesCountAfterDelete);
            fakedContext.ExecutePluginWithConfigurations<DeleteHomeTypePlugin>(pluginContext, "", "");
            var homeTypesCount = fakedContext.CreateQuery<invln_HomeType>()
                                                .Where(x => x.Id == homeTypeId)
                                                .ToList().Count();
            Assert.AreEqual(0, homeTypesCount);
        }
    }
}
